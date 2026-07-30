using Entities;
using FluentAssertions;
using Moq;
using Repository.Contracts;
using Services.Contracts;
using Services.Contracts.Exceptions;
using Services.Contracts.Validation;
using UseCases;

namespace Tests.UseCases;

public class CsvFileProcessingUseCaseTests
{
    private readonly Mock<ICsvParser> parser = new();
    private readonly Mock<ICsvValidator> validator = new();
    private readonly Mock<ICsvAggregator> aggregator = new();
    private readonly Mock<IUnitOfWork> unitOfWork = new();
    private readonly Mock<IValueRepository> valueRepository = new();
    private readonly Mock<IResultRepository> resultRepository = new();

    private readonly CsvFileProcessingUseCase useCase;

    public CsvFileProcessingUseCaseTests()
    {
        unitOfWork.Setup(u => u.Values).Returns(valueRepository.Object);
        unitOfWork.Setup(u => u.Results).Returns(resultRepository.Object);

        // Прогоняем переданное действие "как есть", имитируя реальную транзакцию.
        unitOfWork
            .Setup(u => u.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
            .Returns((Func<Task> action) => action());

        useCase = new CsvFileProcessingUseCase(
            parser.Object,
            validator.Object,
            aggregator.Object,
            unitOfWork.Object);
    }

    private static List<ValueRecord> SampleRows() =>
        [new() { FileName = "f.csv", Date = DateTime.UtcNow, ExecutionTime = 1, Value = 1 }];

    private static ResultRecord SampleResult(string fileName) => new()
    {
        FileName = fileName,
        DeltaSeconds = 0,
        MinDate = DateTime.UtcNow,
        AverageExecutionTime = 1,
        AverageValue = 1,
        MedianValue = 1,
        MaxValue = 1,
        MinValue = 1
    };

    [Fact]
    public async Task ExecuteAsync_HappyPath_ParsesValidatesAggregatesAndSaves()
    {
        var rows = SampleRows();
        var result = SampleResult("f.csv");

        parser.Setup(p => p.ParseAsync(It.IsAny<Stream>(), "f.csv")).ReturnsAsync(rows);
        aggregator.Setup(a => a.Calculate("f.csv", rows)).Returns(result);

        await using var stream = new MemoryStream();
        await useCase.ExecuteAsync(stream, "f.csv");

        parser.Verify(p => p.ParseAsync(stream, "f.csv"), Times.Once);
        validator.Verify(v => v.Validate(rows), Times.Once);
        aggregator.Verify(a => a.Calculate("f.csv", rows), Times.Once);

        valueRepository.Verify(r => r.DeleteByFileNameAsync("f.csv"), Times.Once);
        valueRepository.Verify(r => r.AddRangeAsync(rows), Times.Once);
        resultRepository.Verify(r => r.UpsertAsync(result), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ValidationFails_ThrowsAndDoesNotPersist()
    {
        var rows = SampleRows();
        parser.Setup(p => p.ParseAsync(It.IsAny<Stream>(), "f.csv")).ReturnsAsync(rows);
        validator.Setup(v => v.Validate(rows)).Throws(new CsvValidationException("bad row"));

        await using var stream = new MemoryStream();
        var act = () => useCase.ExecuteAsync(stream, "f.csv");

        await act.Should().ThrowAsync<CsvValidationException>();

        aggregator.Verify(a => a.Calculate(It.IsAny<string>(), It.IsAny<IReadOnlyCollection<ValueRecord>>()), Times.Never);
        valueRepository.Verify(r => r.DeleteByFileNameAsync(It.IsAny<string>()), Times.Never);
        valueRepository.Verify(r => r.AddRangeAsync(It.IsAny<IEnumerable<ValueRecord>>()), Times.Never);
        resultRepository.Verify(r => r.UpsertAsync(It.IsAny<ResultRecord>()), Times.Never);
        unitOfWork.Verify(u => u.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_PersistsValuesAndResultInSameTransaction()
    {
        var rows = SampleRows();
        var result = SampleResult("f.csv");

        parser.Setup(p => p.ParseAsync(It.IsAny<Stream>(), "f.csv")).ReturnsAsync(rows);
        aggregator.Setup(a => a.Calculate("f.csv", rows)).Returns(result);

        await using var stream = new MemoryStream();
        await useCase.ExecuteAsync(stream, "f.csv");

        unitOfWork.Verify(u => u.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeletesOldValuesBeforeAddingNewOnes()
    {
        var rows = SampleRows();
        var result = SampleResult("f.csv");
        var callOrder = new List<string>();

        parser.Setup(p => p.ParseAsync(It.IsAny<Stream>(), "f.csv")).ReturnsAsync(rows);
        aggregator.Setup(a => a.Calculate("f.csv", rows)).Returns(result);

        valueRepository.Setup(r => r.DeleteByFileNameAsync("f.csv"))
            .Callback(() => callOrder.Add("delete"))
            .Returns(Task.CompletedTask);
        valueRepository.Setup(r => r.AddRangeAsync(rows))
            .Callback(() => callOrder.Add("add"))
            .Returns(Task.CompletedTask);

        await using var stream = new MemoryStream();
        await useCase.ExecuteAsync(stream, "f.csv");

        callOrder.Should().Equal("delete", "add");
    }
}
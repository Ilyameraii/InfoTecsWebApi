using Entities;
using FluentAssertions;
using Moq;
using Repository.Contracts;
using Repository.Contracts.Models;
using UseCases;
using UseCases.Contracts.Models;

namespace Tests.UseCases;

public class GetFilteredResultsUseCaseTests
{
    private readonly Mock<IResultRepository> resultRepository = new();
    private readonly GetFilteredResultsUseCase useCase;

    public GetFilteredResultsUseCaseTests()
    {
        useCase = new GetFilteredResultsUseCase(resultRepository.Object);
    }

    [Fact]
    public async Task ExecuteAsync_MapsAllFilterFieldsToRepositoryFilter()
    {
        ResultFilter? capturedFilter = null;

        resultRepository
            .Setup(r => r.GetFilteredAsync(It.IsAny<ResultFilter>()))
            .Callback<ResultFilter>(f => capturedFilter = f)
            .ReturnsAsync([]);

        var request = new ResultFilterRequest
        {
            FileName = "f.csv",
            MinDateFrom = new DateTime(2024, 1, 1),
            MinDateTo = new DateTime(2024, 2, 1),
            AverageValueFrom = 10,
            AverageValueTo = 20,
            AverageExecutionTimeFrom = 1,
            AverageExecutionTimeTo = 5
        };

        await useCase.ExecuteAsync(request);

        capturedFilter.Should().NotBeNull();
        capturedFilter!.FileName.Should().Be(request.FileName);
        capturedFilter.MinDateFrom.Should().Be(request.MinDateFrom);
        capturedFilter.MinDateTo.Should().Be(request.MinDateTo);
        capturedFilter.AverageValueFrom.Should().Be(request.AverageValueFrom);
        capturedFilter.AverageValueTo.Should().Be(request.AverageValueTo);
        capturedFilter.AverageExecutionTimeFrom.Should().Be(request.AverageExecutionTimeFrom);
        capturedFilter.AverageExecutionTimeTo.Should().Be(request.AverageExecutionTimeTo);
    }

    [Fact]
    public async Task ExecuteAsync_EmptyRequest_MapsAllNullsThrough()
    {
        ResultFilter? capturedFilter = null;

        resultRepository
            .Setup(r => r.GetFilteredAsync(It.IsAny<ResultFilter>()))
            .Callback<ResultFilter>(f => capturedFilter = f)
            .ReturnsAsync([]);

        await useCase.ExecuteAsync(new ResultFilterRequest());

        capturedFilter.Should().NotBeNull();
        capturedFilter!.FileName.Should().BeNull();
        capturedFilter.MinDateFrom.Should().BeNull();
        capturedFilter.MinDateTo.Should().BeNull();
        capturedFilter.AverageValueFrom.Should().BeNull();
        capturedFilter.AverageValueTo.Should().BeNull();
        capturedFilter.AverageExecutionTimeFrom.Should().BeNull();
        capturedFilter.AverageExecutionTimeTo.Should().BeNull();
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsRepositoryResultUnchanged()
    {
        var expected = new List<ResultRecord>
        {
            new()
            {
                FileName = "f.csv",
                DeltaSeconds = 1,
                MinDate = DateTime.UtcNow,
                AverageExecutionTime = 1,
                AverageValue = 1,
                MedianValue = 1,
                MaxValue = 1,
                MinValue = 1
            }
        };

        resultRepository.Setup(r => r.GetFilteredAsync(It.IsAny<ResultFilter>())).ReturnsAsync(expected);

        var result = await useCase.ExecuteAsync(new ResultFilterRequest());

        result.Should().BeEquivalentTo(expected);
    }
}
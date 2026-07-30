using System.Text;
using FluentAssertions;
using Services;
using Services.Contracts.Exceptions;

namespace Tests.Services;

public class CsvParserTests
{
    private readonly CsvParser csvParser = new();

    private const string FileName = "test.csv";

    private static Stream ToStream(string content) =>
        new MemoryStream(Encoding.UTF8.GetBytes(content));

    [Fact]
    public async Task ParseAsync_ShouldSkipHeaderLine()
    {
        // Arrange
        var csv = "Date;ExecutionTime;Value\r\n" +
                   "2024-01-01T10-00-00.0000Z;1.5;10.5\r\n";

        // Act
        var result = await csvParser.ParseAsync(ToStream(csv), FileName);

        // Assert
        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task ParseAsync_ShouldSkipEmptyLines()
    {
        // Arrange
        var csv = "Date;ExecutionTime;Value\r\n" +
                   "2024-01-01T10-00-00.0000Z;1.5;10.5\r\n" +
                   "\r\n" +
                   "2024-01-02T10-00-00.0000Z;2.5;20.5\r\n";

        // Act
        var result = await csvParser.ParseAsync(ToStream(csv), FileName);

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task ParseAsync_ShouldCorrectlyMapAllFields()
    {
        // Arrange
        var csv = "Date;ExecutionTime;Value\r\n" +
                   "2024-01-01T10-30-15.1234Z;1.5;10.75\r\n";

        // Act
        var result = await csvParser.ParseAsync(ToStream(csv), FileName);

        // Assert
        var row = result.Single();
        row.FileName.Should().Be(FileName);
        row.Date.Should().Be(new DateTime(2024, 1, 1, 10, 30, 15, 123, DateTimeKind.Utc).AddTicks(4000));
        row.ExecutionTime.Should().Be(1.5);
        row.Value.Should().Be(10.75);
    }

    [Fact]
    public async Task ParseAsync_WithWrongFieldCount_ShouldThrowCsvValidationException()
    {
        // Arrange
        var csv = "Date;ExecutionTime;Value\r\n" +
                   "2024-01-01T10-00-00.0000Z;1.5\r\n";

        // Act
        var act = async () => await csvParser.ParseAsync(ToStream(csv), FileName);

        // Assert
        await act.Should().ThrowAsync<CsvValidationException>();
    }

    [Theory]
    [InlineData(";1.5;10.5")]
    [InlineData("2024-01-01T10-00-00.0000Z;;10.5")]
    [InlineData("2024-01-01T10-00-00.0000Z;1.5;")]
    public async Task ParseAsync_WithMissingValue_ShouldThrowCsvValidationException(string dataLine)
    {
        // Arrange
        var csv = $"Date;ExecutionTime;Value\r\n{dataLine}\r\n";

        // Act
        var act = async () => await csvParser.ParseAsync(ToStream(csv), FileName);

        // Assert
        await act.Should().ThrowAsync<CsvValidationException>();
    }

    [Fact]
    public async Task ParseAsync_WithInvalidDateFormat_ShouldThrowCsvValidationException()
    {
        // Arrange
        var csv = "Date;ExecutionTime;Value\r\n" +
                   "not-a-date;1.5;10.5\r\n";

        // Act
        var act = async () => await csvParser.ParseAsync(ToStream(csv), FileName);

        // Assert
        await act.Should().ThrowAsync<CsvValidationException>();
    }

    [Fact]
    public async Task ParseAsync_WithInvalidExecutionTime_ShouldThrowCsvValidationException()
    {
        // Arrange
        var csv = "Date;ExecutionTime;Value\r\n" +
                   "2024-01-01T10-00-00.0000Z;not-a-number;10.5\r\n";

        // Act
        var act = async () => await csvParser.ParseAsync(ToStream(csv), FileName);

        // Assert
        await act.Should().ThrowAsync<CsvValidationException>();
    }

    [Fact]
    public async Task ParseAsync_WithInvalidValue_ShouldThrowCsvValidationException()
    {
        // Arrange
        var csv = "Date;ExecutionTime;Value\r\n" +
                   "2024-01-01T10-00-00.0000Z;1.5;not-a-number\r\n";

        // Act
        var act = async () => await csvParser.ParseAsync(ToStream(csv), FileName);

        // Assert
        await act.Should().ThrowAsync<CsvValidationException>();
    }

    [Fact]
    public async Task ParseAsync_WithOnlyHeader_ShouldReturnEmptyCollection()
    {
        // Arrange
        var csv = "Date;ExecutionTime;Value\r\n";

        // Act
        var result = await csvParser.ParseAsync(ToStream(csv), FileName);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ParseAsync_ShouldParseMultipleRowsInOrder()
    {
        // Arrange
        var csv = "Date;ExecutionTime;Value\r\n" +
                   "2024-01-01T10-00-00.0000Z;1;10\r\n" +
                   "2024-01-02T10-00-00.0000Z;2;20\r\n" +
                   "2024-01-03T10-00-00.0000Z;3;30\r\n";

        // Act
        var result = await csvParser.ParseAsync(ToStream(csv), FileName);

        // Assert
        result.Should().HaveCount(3);
        result.Select(r => r.Value).Should().ContainInOrder(10, 20, 30);
    }
}
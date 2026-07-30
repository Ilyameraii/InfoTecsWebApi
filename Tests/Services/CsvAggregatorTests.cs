using Entities;
using FluentAssertions;
using Services;

namespace Tests.Services;

public class CsvAggregatorTests
{
    private readonly CsvAggregator csvAggregator = new();

    private static ValueRecord CreateRow(string fileName, DateTime date, double executionTime, double value) =>
        new()
        {
            FileName = fileName,
            Date = date,
            ExecutionTime = executionTime,
            Value = value
        };

    [Fact]
    public void Calculate_ShouldSetFileName_FromParameter()
    {
        // Arrange
        var rows = new List<ValueRecord>
        {
            CreateRow("file.csv", new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), 1, 10)
        };

        // Act
        var result = csvAggregator.Calculate("file.csv", rows);

        // Assert
        result.FileName.Should().Be("file.csv");
    }

    [Fact]
    public void Calculate_ShouldComputeDeltaSecondsAndMinDate_FromDateRange()
    {
        // Arrange
        var minDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var maxDate = minDate.AddSeconds(120);

        var rows = new List<ValueRecord>
        {
            CreateRow("file.csv", minDate, 1, 10),
            CreateRow("file.csv", maxDate, 2, 20)
        };

        // Act
        var result = csvAggregator.Calculate("file.csv", rows);

        // Assert
        result.MinDate.Should().Be(minDate);
        result.DeltaSeconds.Should().Be(120);
    }

    [Fact]
    public void Calculate_ShouldComputeAverageExecutionTime()
    {
        // Arrange
        var date = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var rows = new List<ValueRecord>
        {
            CreateRow("file.csv", date, 10, 1),
            CreateRow("file.csv", date, 20, 1),
            CreateRow("file.csv", date, 30, 1)
        };

        // Act
        var result = csvAggregator.Calculate("file.csv", rows);

        // Assert
        result.AverageExecutionTime.Should().Be(20);
    }

    [Fact]
    public void Calculate_ShouldComputeMinMaxAndAverageValue()
    {
        // Arrange
        var date = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var rows = new List<ValueRecord>
        {
            CreateRow("file.csv", date, 1, 5),
            CreateRow("file.csv", date, 1, 15),
            CreateRow("file.csv", date, 1, 10)
        };

        // Act
        var result = csvAggregator.Calculate("file.csv", rows);

        // Assert
        result.MinValue.Should().Be(5);
        result.MaxValue.Should().Be(15);
        result.AverageValue.Should().Be(10);
    }

    [Fact]
    public void Calculate_WithOddNumberOfValues_ShouldComputeMedianAsMiddleElement()
    {
        // Arrange
        var date = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var rows = new List<ValueRecord>
        {
            CreateRow("file.csv", date, 1, 30),
            CreateRow("file.csv", date, 1, 10),
            CreateRow("file.csv", date, 1, 20)
        };

        // Act
        var result = csvAggregator.Calculate("file.csv", rows);

        // Assert
        result.MedianValue.Should().Be(20);
    }

    [Fact]
    public void Calculate_WithEvenNumberOfValues_ShouldComputeMedianAsAverageOfTwoMiddleElements()
    {
        // Arrange
        var date = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var rows = new List<ValueRecord>
        {
            CreateRow("file.csv", date, 1, 10),
            CreateRow("file.csv", date, 1, 20),
            CreateRow("file.csv", date, 1, 30),
            CreateRow("file.csv", date, 1, 40)
        };

        // Act
        var result = csvAggregator.Calculate("file.csv", rows);

        // Assert
        result.MedianValue.Should().Be(25);
    }

    [Fact]
    public void Calculate_WithSingleRow_ShouldReturnZeroDeltaAndSameValueForAllAggregates()
    {
        // Arrange
        var date = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var rows = new List<ValueRecord>
        {
            CreateRow("file.csv", date, 5, 42)
        };

        // Act
        var result = csvAggregator.Calculate("file.csv", rows);

        // Assert
        result.DeltaSeconds.Should().Be(0);
        result.MinValue.Should().Be(42);
        result.MaxValue.Should().Be(42);
        result.AverageValue.Should().Be(42);
        result.MedianValue.Should().Be(42);
        result.AverageExecutionTime.Should().Be(5);
    }
}
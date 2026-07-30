using Entities;
using FluentAssertions;
using Repositories.Filtering;
using Repository.Contracts.Models;

namespace Tests.Filtering;

public class FileNameFilterStrategyTests
{
    private readonly FileNameFilterStrategy strategy = new();

    private static ResultRecord Result(string fileName) => new()
    {
        FileName = fileName,
        DeltaSeconds = 0,
        MinDate = DateTime.UtcNow,
        AverageExecutionTime = 0,
        AverageValue = 0,
        MedianValue = 0,
        MaxValue = 0,
        MinValue = 0
    };

    [Fact]
    public void IsApplicable_FileNameSet_ReturnsTrue()
    {
        strategy.IsApplicable(new ResultFilter { FileName = "a.csv" }).Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void IsApplicable_FileNameNullOrWhitespace_ReturnsFalse(string? fileName)
    {
        strategy.IsApplicable(new ResultFilter { FileName = fileName }).Should().BeFalse();
    }

    [Fact]
    public void Apply_FiltersByExactFileNameMatch()
    {
        var data = new List<ResultRecord> { Result("a.csv"), Result("b.csv") }.AsQueryable();

        var filtered = strategy.Apply(data, new ResultFilter { FileName = "a.csv" }).ToList();

        filtered.Should().ContainSingle();
        filtered[0].FileName.Should().Be("a.csv");
    }

    [Fact]
    public void Apply_NoMatch_ReturnsEmpty()
    {
        var data = new List<ResultRecord> { Result("a.csv") }.AsQueryable();

        var filtered = strategy.Apply(data, new ResultFilter { FileName = "missing.csv" }).ToList();

        filtered.Should().BeEmpty();
    }
}
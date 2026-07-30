using Entities;
using FluentAssertions;
using Repositories.Filtering;
using Repository.Contracts.Models;

namespace Tests.Filtering;

public class AverageValueFilterStrategyTests
{
    private readonly AverageValueFilterStrategy strategy = new();

    private static ResultRecord Result(string fileName, double averageValue) => new()
    {
        FileName = fileName,
        DeltaSeconds = 0,
        MinDate = DateTime.UtcNow,
        AverageExecutionTime = 0,
        AverageValue = averageValue,
        MedianValue = 0,
        MaxValue = 0,
        MinValue = 0
    };

    [Fact]
    public void IsApplicable_NoRangeSet_ReturnsFalse()
    {
        strategy.IsApplicable(new ResultFilter()).Should().BeFalse();
    }

    [Fact]
    public void IsApplicable_RangeSet_ReturnsTrue()
    {
        strategy.IsApplicable(new ResultFilter { AverageValueFrom = 1 }).Should().BeTrue();
        strategy.IsApplicable(new ResultFilter { AverageValueTo = 1 }).Should().BeTrue();
    }

    [Fact]
    public void Apply_FiltersInclusiveRange()
    {
        var data = new List<ResultRecord>
        {
            Result("a", 10),
            Result("b", 20),
            Result("c", 30)
        }.AsQueryable();

        var filtered = strategy.Apply(data, new ResultFilter { AverageValueFrom = 10, AverageValueTo = 20 }).ToList();

        filtered.Select(r => r.FileName).Should().BeEquivalentTo(["a", "b"]);
    }

    [Fact]
    public void Apply_BoundaryValues_AreInclusive()
    {
        var data = new List<ResultRecord> { Result("a", 10) }.AsQueryable();

        strategy.Apply(data, new ResultFilter { AverageValueFrom = 10 }).Should().ContainSingle();
        strategy.Apply(data, new ResultFilter { AverageValueTo = 10 }).Should().ContainSingle();
    }
}
using Entities;
using FluentAssertions;
using Repositories.Filtering;
using Repository.Contracts.Models;

namespace Tests.Filtering;

public class AverageExecutionTimeFilterStrategyTests
{
    private readonly AverageExecutionTimeFilterStrategy strategy = new();
 
    private static ResultRecord Result(string fileName, double averageExecutionTime) => new()
    {
        FileName = fileName,
        DeltaSeconds = 0,
        MinDate = DateTime.UtcNow,
        AverageExecutionTime = averageExecutionTime,
        AverageValue = 0,
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
        strategy.IsApplicable(new ResultFilter { AverageExecutionTimeFrom = 1 }).Should().BeTrue();
        strategy.IsApplicable(new ResultFilter { AverageExecutionTimeTo = 1 }).Should().BeTrue();
    }
 
    [Fact]
    public void Apply_FiltersInclusiveRange()
    {
        var data = new List<ResultRecord>
        {
            Result("a", 1),
            Result("b", 2),
            Result("c", 3)
        }.AsQueryable();
 
        var filtered = strategy
            .Apply(data, new ResultFilter { AverageExecutionTimeFrom = 2, AverageExecutionTimeTo = 3 })
            .ToList();
 
        filtered.Select(r => r.FileName).Should().BeEquivalentTo(["b", "c"]);
    }
}
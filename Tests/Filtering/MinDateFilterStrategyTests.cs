using Entities;
using FluentAssertions;
using Repositories.Filtering;
using Repository.Contracts.Models;

namespace Tests.Filtering;

public class MinDateFilterStrategyTests
{
    private readonly MinDateFilterStrategy strategy = new();

    private static ResultRecord Result(string fileName, DateTime minDate) => new()
    {
        FileName = fileName,
        DeltaSeconds = 0,
        MinDate = minDate,
        AverageExecutionTime = 0,
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
    public void IsApplicable_FromOnly_ReturnsTrue()
    {
        strategy.IsApplicable(new ResultFilter { MinDateFrom = DateTime.UtcNow }).Should().BeTrue();
    }

    [Fact]
    public void IsApplicable_ToOnly_ReturnsTrue()
    {
        strategy.IsApplicable(new ResultFilter { MinDateTo = DateTime.UtcNow }).Should().BeTrue();
    }

    [Fact]
    public void Apply_FromAndTo_FiltersInclusiveRange()
    {
        var day1 = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var day2 = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc);
        var day3 = new DateTime(2024, 1, 3, 0, 0, 0, DateTimeKind.Utc);

        var data = new List<ResultRecord>
        {
            Result("a", day1),
            Result("b", day2),
            Result("c", day3)
        }.AsQueryable();

        var filter = new ResultFilter { MinDateFrom = day1, MinDateTo = day2 };

        var filtered = strategy.Apply(data, filter).ToList();

        filtered.Should().HaveCount(2);
        filtered.Select(r => r.FileName).Should().BeEquivalentTo(["a", "b"]);
    }

    [Fact]
    public void Apply_OnlyFrom_ExcludesEarlierDates()
    {
        var day1 = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var day2 = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc);

        var data = new List<ResultRecord> { Result("a", day1), Result("b", day2) }.AsQueryable();

        var filtered = strategy.Apply(data, new ResultFilter { MinDateFrom = day2 }).ToList();

        filtered.Should().ContainSingle();
        filtered[0].FileName.Should().Be("b");
    }

    [Fact]
    public void Apply_OnlyTo_ExcludesLaterDates()
    {
        var day1 = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var day2 = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc);

        var data = new List<ResultRecord> { Result("a", day1), Result("b", day2) }.AsQueryable();

        var filtered = strategy.Apply(data, new ResultFilter { MinDateTo = day1 }).ToList();

        filtered.Should().ContainSingle();
        filtered[0].FileName.Should().Be("a");
    }
}
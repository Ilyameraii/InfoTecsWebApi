using Entities;
using FluentAssertions;
using Services.Validation.Rules;

namespace Tests.Services.ValidationRules;

public class RowCountRuleTests
{
    private readonly RowCountRule rule = new();
 
    private static List<ValueRecord> CreateRows(int count) =>
        Enumerable.Range(0, count)
            .Select(_ => new ValueRecord
            {
                FileName = "file.csv",
                Date = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ExecutionTime = 1,
                Value = 1
            })
            .ToList();
 
    [Fact]
    public void IsValid_WithZeroRows_ShouldReturnFalseWithError()
    {
        // Arrange
        var rows = CreateRows(0);
 
        // Act
        var isValid = rule.IsValid(rows, out var error);
 
        // Assert
        isValid.Should().BeFalse();
        error.Should().NotBeNullOrEmpty();
    }
 
    [Fact]
    public void IsValid_WithOneRow_ShouldReturnTrue()
    {
        // Arrange
        var rows = CreateRows(1);
 
        // Act
        var isValid = rule.IsValid(rows, out var error);
 
        // Assert
        isValid.Should().BeTrue();
        error.Should().BeNull();
    }
 
    [Fact]
    public void IsValid_With10000Rows_ShouldReturnTrue()
    {
        // Arrange
        var rows = CreateRows(10_000);
 
        // Act
        var isValid = rule.IsValid(rows, out var error);
 
        // Assert
        isValid.Should().BeTrue();
        error.Should().BeNull();
    }
 
    [Fact]
    public void IsValid_With10001Rows_ShouldReturnFalseWithError()
    {
        // Arrange
        var rows = CreateRows(10_001);
 
        // Act
        var isValid = rule.IsValid(rows, out var error);
 
        // Assert
        isValid.Should().BeFalse();
        error.Should().NotBeNullOrEmpty();
    }
}
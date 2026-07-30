using Entities;
using FluentAssertions;
using Services.Validation.Rules;

namespace Tests.Services.ValidationRules;

public class DateNotInFutureRuleTests
{
    private readonly DateNotInFutureRule rule = new();
 
    private static ValueRecord CreateRow(DateTime date) =>
        new() { FileName = "file.csv", Date = date, ExecutionTime = 1, Value = 1 };
 
    [Fact]
    public void IsValid_WithPastDate_ShouldReturnTrue()
    {
        // Arrange
        var row = CreateRow(DateTime.UtcNow.AddDays(-1));
 
        // Act
        var isValid = rule.IsValid(row, out var error);
 
        // Assert
        isValid.Should().BeTrue();
        error.Should().BeNull();
    }
 
    [Fact]
    public void IsValid_WithFutureDate_ShouldReturnFalseWithError()
    {
        // Arrange
        var row = CreateRow(DateTime.UtcNow.AddDays(1));
 
        // Act
        var isValid = rule.IsValid(row, out var error);
 
        // Assert
        isValid.Should().BeFalse();
        error.Should().NotBeNullOrEmpty();
    }
}
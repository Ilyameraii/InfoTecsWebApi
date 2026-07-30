using Entities;
using FluentAssertions;
using Services.Validation.Rules;

namespace Tests.Services.ValidationRules;

public class DateNotBeforeMinRuleTests
{
    private readonly DateNotBeforeMinRule rule = new();
 
    private static ValueRecord CreateRow(DateTime date) =>
        new() { FileName = "file.csv", Date = date, ExecutionTime = 1, Value = 1 };
 
    [Fact]
    public void IsValid_WithDateEqualToMinDate_ShouldReturnTrue()
    {
        // Arrange
        var row = CreateRow(new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc));
 
        // Act
        var isValid = rule.IsValid(row, out var error);
 
        // Assert
        isValid.Should().BeTrue();
        error.Should().BeNull();
    }
 
    [Fact]
    public void IsValid_WithDateAfterMinDate_ShouldReturnTrue()
    {
        // Arrange
        var row = CreateRow(new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc));
 
        // Act
        var isValid = rule.IsValid(row, out var error);
 
        // Assert
        isValid.Should().BeTrue();
    }
 
    [Fact]
    public void IsValid_WithDateBeforeMinDate_ShouldReturnFalseWithError()
    {
        // Arrange
        var row = CreateRow(new DateTime(1999, 12, 31, 0, 0, 0, DateTimeKind.Utc));
 
        // Act
        var isValid = rule.IsValid(row, out var error);
 
        // Assert
        isValid.Should().BeFalse();
        error.Should().NotBeNullOrEmpty();
    }
}
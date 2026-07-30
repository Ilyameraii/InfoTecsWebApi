using Entities;
using FluentAssertions;
using Services.Validation.Rules;

namespace Tests.Services.ValidationRules;

public class ExecutionTimeNonNegativeRuleTests
{
    private readonly ExecutionTimeNonNegativeRule rule = new();
 
    private static ValueRecord CreateRow(double executionTime) =>
        new()
        {
            FileName = "file.csv",
            Date = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            ExecutionTime = executionTime,
            Value = 1
        };
 
    [Theory]
    [InlineData(0)]
    [InlineData(1.5)]
    [InlineData(1000)]
    public void IsValid_WithNonNegativeExecutionTime_ShouldReturnTrue(double executionTime)
    {
        // Arrange
        var row = CreateRow(executionTime);
 
        // Act
        var isValid = rule.IsValid(row, out var error);
 
        // Assert
        isValid.Should().BeTrue();
        error.Should().BeNull();
    }
 
    [Fact]
    public void IsValid_WithNegativeExecutionTime_ShouldReturnFalseWithError()
    {
        // Arrange
        var row = CreateRow(-0.01);
 
        // Act
        var isValid = rule.IsValid(row, out var error);
 
        // Assert
        isValid.Should().BeFalse();
        error.Should().NotBeNullOrEmpty();
    }
}
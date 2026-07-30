using Entities;
using FluentAssertions;
using Services.Validation.Rules;

namespace Tests.Services.ValidationRules;

public class ValueNonNegativeRuleTests
{
    private readonly ValueNonNegativeRule rule = new();
 
    private static ValueRecord CreateRow(double value) =>
        new()
        {
            FileName = "file.csv",
            Date = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            ExecutionTime = 1,
            Value = value
        };
 
    [Theory]
    [InlineData(0)]
    [InlineData(0.5)]
    [InlineData(9999.99)]
    public void IsValid_WithNonNegativeValue_ShouldReturnTrue(double value)
    {
        // Arrange
        var row = CreateRow(value);
 
        // Act
        var isValid = rule.IsValid(row, out var error);
 
        // Assert
        isValid.Should().BeTrue();
        error.Should().BeNull();
    }
 
    [Fact]
    public void IsValid_WithNegativeValue_ShouldReturnFalseWithError()
    {
        // Arrange
        var row = CreateRow(-1);
 
        // Act
        var isValid = rule.IsValid(row, out var error);
 
        // Assert
        isValid.Should().BeFalse();
        error.Should().NotBeNullOrEmpty();
    }
}
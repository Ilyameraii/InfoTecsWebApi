using Entities;
using FluentAssertions;
using Services.Contracts.Exceptions;
using Services.Contracts.Validation;
using Services.Validation;

namespace Tests.Services;

public class CsvValidatorTests
{
    private static ValueRecord CreateRow(DateTime date, double executionTime, double value) =>
        new()
        {
            FileName = "file.csv",
            Date = date,
            ExecutionTime = executionTime,
            Value = value
        };

    private static readonly DateTime ValidDate = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private class AlwaysValidRowRule : IValueValidationRule
    {
        public bool IsValid(ValueRecord item, out string? error)
        {
            error = null;
            return true;
        }
    }

    private class AlwaysInvalidRowRule(string message) : IValueValidationRule
    {
        public bool IsValid(ValueRecord item, out string? error)
        {
            error = message;
            return false;
        }
    }

    private class AlwaysValidCollectionRule : IValueCollectionValidationRule
    {
        public bool IsValid(IReadOnlyCollection<ValueRecord> items, out string? error)
        {
            error = null;
            return true;
        }
    }

    private class AlwaysInvalidCollectionRule(string message) : IValueCollectionValidationRule
    {
        public bool IsValid(IReadOnlyCollection<ValueRecord> items, out string? error)
        {
            error = message;
            return false;
        }
    }

    [Fact]
    public void Validate_WhenAllRulesPass_ShouldNotThrow()
    {
        // Arrange
        var csvValidator = new CsvValidator(
            [new AlwaysValidRowRule()],
            [new AlwaysValidCollectionRule()]);

        var rows = new List<ValueRecord> { CreateRow(ValidDate, 1, 1) };

        // Act
        var act = () => csvValidator.Validate(rows);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WhenRowRuleFails_ShouldThrowCsvValidationExceptionWithLineNumber()
    {
        // Arrange
        var csvValidator = new CsvValidator(
            [new AlwaysInvalidRowRule("row is invalid")],
            []);

        var rows = new List<ValueRecord> { CreateRow(ValidDate, 1, 1) };

        // Act
        var act = () => csvValidator.Validate(rows);

        // Assert
        act.Should().Throw<CsvValidationException>()
            .Which.Errors.Should().ContainSingle(e => e.Contains("Строка 1") && e.Contains("row is invalid"));
    }

    [Fact]
    public void Validate_WhenCollectionRuleFails_ShouldThrowCsvValidationException()
    {
        // Arrange
        var csvValidator = new CsvValidator(
            [],
            [new AlwaysInvalidCollectionRule("collection is invalid")]);

        var rows = new List<ValueRecord> { CreateRow(ValidDate, 1, 1) };

        // Act
        var act = () => csvValidator.Validate(rows);

        // Assert
        act.Should().Throw<CsvValidationException>()
            .Which.Errors.Should().ContainSingle(e => e == "collection is invalid");
    }

    [Fact]
    public void Validate_WhenMultipleRulesFail_ShouldAggregateAllErrors()
    {
        // Arrange
        var csvValidator = new CsvValidator(
            [new AlwaysInvalidRowRule("row error")],
            [new AlwaysInvalidCollectionRule("collection error")]);

        var rows = new List<ValueRecord>
        {
            CreateRow(ValidDate, 1, 1),
            CreateRow(ValidDate, 1, 1)
        };

        // Act
        var act = () => csvValidator.Validate(rows);

        // Assert
        act.Should().Throw<CsvValidationException>()
            .Which.Errors.Should().HaveCount(3); // 1 collection error + 2 row errors
    }

    [Fact]
    public void Validate_ShouldReportCorrectLineNumberForEachFailingRow()
    {
        // Arrange
        var csvValidator = new CsvValidator(
            [new AlwaysInvalidRowRule("bad row")],
            []);

        var rows = new List<ValueRecord>
        {
            CreateRow(ValidDate, 1, 1),
            CreateRow(ValidDate, 1, 1),
            CreateRow(ValidDate, 1, 1)
        };

        // Act
        var act = () => csvValidator.Validate(rows);

        // Assert
        act.Should().Throw<CsvValidationException>()
            .Which.Errors.Should().BeEquivalentTo(
            [
                "Строка 1: bad row",
                "Строка 2: bad row",
                "Строка 3: bad row"
            ]);
    }
}
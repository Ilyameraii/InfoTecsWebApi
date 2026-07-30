using Entities;
using Services.Contracts.Exceptions;
using Services.Contracts.Validation;

namespace Services.Validation;

/// <summary>
/// Реализация <see cref="ICsvValidator"/>.
/// Последовательно применяет правила валидации коллекции (<see cref="IValueCollectionValidationRule"/>)
/// и правила валидации отдельных строк (<see cref="IValueValidationRule"/>), собирая все найденные
/// ошибки, и выбрасывает <see cref="CsvValidationException"/> с полным списком ошибок, если они есть.
/// </summary>
public class CsvValidator(
    IEnumerable<IValueValidationRule> rowRules,
    IEnumerable<IValueCollectionValidationRule> collectionRules) : ICsvValidator
{
    /// <inheritdoc/>
    public void Validate(IReadOnlyCollection<ValueRecord> rows)
    {
        var errors = new List<string>();

        foreach (var rule in collectionRules)
        {
            if (!rule.IsValid(rows, out var error))
            {
                errors.Add(error!);
            }
        }

        var lineNumber = 0;

        foreach (var row in rows)
        {
            lineNumber++;

            foreach (var rule in rowRules)
            {
                if (!rule.IsValid(row, out var error))
                {
                    errors.Add($"Строка {lineNumber}: {error}");
                }
            }
        }

        if (errors.Count > 0)
        {
            throw new CsvValidationException(errors);
        }
    }
}
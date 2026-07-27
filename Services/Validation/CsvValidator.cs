using Entities;
using Services.Contracts.Exceptions;
using Services.Contracts.Validation;

namespace Services.Validation;

public class CsvValidator(
    IEnumerable<IValueValidationRule> rowRules,
    IEnumerable<IValueCollectionValidationRule> collectionRules) : ICsvValidator
{
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
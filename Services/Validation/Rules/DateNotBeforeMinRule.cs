using Entities;
using Services.Contracts.Validation;

namespace Services.Validation.Rules;

public class DateNotBeforeMinRule : IValueValidationRule
{
    private static readonly DateTime MinDate = new(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <inheritdoc/>
    public bool IsValid(ValueRecord item, out string? error)
    {
        if (item.Date < MinDate)
        {
            error = $"Дата '{item.Date:O}' не может быть раньше {MinDate:O}";
            return false;
        }

        error = null;
        return true;
    }
}
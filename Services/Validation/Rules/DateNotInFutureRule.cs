using Entities;
using Services.Contracts.Validation;

namespace Services.Validation.Rules;

public class DateNotInFutureRule : IValueValidationRule
{
    /// <inheritdoc/>
    public bool IsValid(ValueRecord item, out string? error)
    {
        if (item.Date > DateTime.UtcNow)
        {
            error = $"Дата '{item.Date:O}' не может быть позже текущей";
            return false;
        }

        error = null;
        return true;
    }
}
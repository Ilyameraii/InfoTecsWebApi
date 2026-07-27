using Entities;
using Services.Contracts.Validation;

namespace Services.Validation.Rules;

public class ValueNonNegativeRule : IValueValidationRule
{
    /// <inheritdoc/>
    public bool IsValid(ValueRecord item, out string? error)
    {
        if (item.Value < 0)
        {
            error = $"Значение показателя не может быть меньше 0 (получено {item.Value})";
            return false;
        }

        error = null;
        return true;
    }
}
using Entities;
using Services.Contracts.Validation;

namespace Services.Validation.Rules;

public class ExecutionTimeNonNegativeRule : IValueValidationRule
{
    /// <inheritdoc/>
    public bool IsValid(ValueRecord item, out string? error)
    {
        if (item.ExecutionTime < 0)
        {
            error = $"Время выполнения не может быть меньше 0 (получено {item.ExecutionTime})";
            return false;
        }

        error = null;
        return true;
    }
}
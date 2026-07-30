using Entities;
using Services.Contracts.Validation;

namespace Services.Validation.Rules;

/// <summary>
/// Реализация <see cref="IValueValidationRule"/>.
/// Проверяет, что дата записи не позже текущего момента времени (UTC).
/// </summary>
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
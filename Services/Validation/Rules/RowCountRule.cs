using Entities;
using Services.Contracts.Validation;

namespace Services.Validation.Rules;

/// <summary>
/// Реализация <see cref="IValueCollectionValidationRule"/>.
/// Проверяет, что количество строк в файле находится в допустимом диапазоне
/// от <see cref="MinRows"/> до <see cref="MaxRows"/> включительно.
/// </summary>
public class RowCountRule : IValueCollectionValidationRule
{
    private const int MinRows = 1;
    private const int MaxRows = 10_000;

    /// <inheritdoc/>
    public bool IsValid(IReadOnlyCollection<ValueRecord> items, out string? error)
    {
        if (items.Count < MinRows || items.Count > MaxRows)
        {
            error = $"Количество строк должно быть от {MinRows} до {MaxRows} (получено {items.Count})";
            return false;
        }

        error = null;
        return true;
    }
}
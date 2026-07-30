using Entities;

namespace Services.Contracts.Validation;

/// <summary>
/// Определяет правило валидации, применяемое к коллекции распарсенных строк CSV-файла
/// в целом (в отличие от <see cref="IValueValidationRule"/>, проверяющего отдельную запись).
/// </summary>
public interface IValueCollectionValidationRule
{
    /// <summary>
    /// Проверяет коллекцию записей на соответствие правилу.
    /// </summary>
    /// <param name="items">Проверяемая коллекция записей.</param>
    /// <param name="error">
    /// Текст ошибки, если коллекция не прошла проверку; в противном случае <c>null</c>.
    /// </param>
    /// <returns><c>true</c>, если коллекция удовлетворяет правилу; иначе <c>false</c>.</returns>
    bool IsValid(IReadOnlyCollection<ValueRecord> items, out string? error);
}
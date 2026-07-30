using Entities;

namespace Services.Contracts.Validation;

/// <summary>
/// Определяет правило валидации, применяемое к отдельной записи <see cref="ValueRecord"/>
/// (в отличие от <see cref="IValueCollectionValidationRule"/>, проверяющего коллекцию в целом).
/// </summary>
public interface IValueValidationRule
{
    /// <summary>
    /// Проверяет отдельную запись на соответствие правилу.
    /// </summary>
    /// <param name="item">Проверяемая запись.</param>
    /// <param name="error">
    /// Текст ошибки, если запись не прошла проверку; в противном случае <c>null</c>.
    /// </param>
    /// <returns><c>true</c>, если запись удовлетворяет правилу; иначе <c>false</c>.</returns>
    bool IsValid(ValueRecord item, out string? error);
}
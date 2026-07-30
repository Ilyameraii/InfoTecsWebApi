using Entities;

namespace Services.Contracts.Validation;

/// <summary>
/// Предоставляет функциональность валидации коллекции распарсенных строк CSV-файла
/// на соответствие бизнес-правилам, применяемым как к отдельным строкам, так и к коллекции в целом.
/// </summary>
public interface ICsvValidator
{
    /// <summary>
    /// Проверяет коллекцию записей на соответствие правилам валидации.
    /// </summary>
    /// <param name="rows">Коллекция распарсенных строк CSV-файла, подлежащих проверке.</param>
    void Validate(IReadOnlyCollection<ValueRecord> rows);
}
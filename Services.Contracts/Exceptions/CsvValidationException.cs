namespace Services.Contracts.Exceptions;

/// <summary>
/// Исключение, возникающее при обнаружении одной или нескольких ошибок валидации
/// содержимого CSV-файла (некорректный формат, недопустимые значения, нарушение
/// бизнес-правил и т.д.).
/// </summary>
/// <param name="errors">Список текстовых описаний найденных ошибок валидации.</param>
public class CsvValidationException(IReadOnlyList<string> errors)
    : Exception($"CSV содержит ошибки: {string.Join("; ", errors)}")
{
    /// <summary>
    /// Список текстовых описаний всех ошибок валидации, приведших к исключению.
    /// </summary>
    public IReadOnlyList<string> Errors => errors;

    /// <summary>
    /// Создаёт исключение с единственной ошибкой валидации.
    /// </summary>
    /// <param name="error">Текстовое описание ошибки валидации.</param>
    public CsvValidationException(string error)
        : this([error])
    {
    }
}
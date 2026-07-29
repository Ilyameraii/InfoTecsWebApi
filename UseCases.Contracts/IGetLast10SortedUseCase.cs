using Entities;

namespace UseCases.Contracts;

/// <summary>
/// Use case получения последних 10 значений по указанному файлу,
/// отсортированных по времени запуска (Date).
/// </summary>
public interface IGetLast10SortedUseCase
{
    /// <summary>
    /// Возвращает последние 10 записей для заданного файла.
    /// </summary>
    /// <param name="fileName">Имя файла, по которому нужно получить значения.</param>
    /// <returns>Список последних 10 записей <see cref="ValueRecord"/>.</returns>
    Task<IReadOnlyList<ValueRecord>> ExecuteAsync(string fileName);
}
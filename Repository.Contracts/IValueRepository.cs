using Entities;

namespace Repository.Contracts;

/// <summary>
/// Репозиторий сырых значений, полученных из CSV-файлов (таблица Values).
/// Предоставляет операции удаления значений по имени файла, массовой вставки
/// и выборки последних значений для заданного файла.
/// </summary>
public interface IValueRepository
{
    /// <summary>
    /// Удаляет все значения, ранее сохранённые для файла с указанным именем.
    /// Используется при перезаписи данных ранее обработанного файла.
    /// </summary>
    /// <param name="fileName">Имя файла, значения которого необходимо удалить.</param>
    Task DeleteByFileNameAsync(string fileName);

    /// <summary>
    /// Добавляет набор значений, полученных из CSV-файла.
    /// </summary>
    /// <param name="values">Коллекция значений, подлежащих сохранению.</param>
    Task AddRangeAsync(IEnumerable<ValueRecord> values);

    /// <summary>
    /// Возвращает последние 10 значений для указанного файла, отсортированные
    /// по времени начала операции (Date) в порядке убывания.
    /// </summary>
    /// <param name="fileName">Имя файла, значения которого необходимо получить.</param>
    /// <returns>Список из не более чем 10 последних значений.</returns>
    Task<IReadOnlyList<ValueRecord>> GetLast10SortedAsync(string fileName);
}
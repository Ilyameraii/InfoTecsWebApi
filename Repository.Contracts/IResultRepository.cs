using Entities;
using Repository.Contracts.Models;

namespace Repository.Contracts;

/// <summary>
/// Репозиторий агрегированных результатов обработки CSV-файлов (таблица Results).
/// Предоставляет операции вставки/обновления агрегата по имени файла и выборки
/// результатов с применением фильтров.
/// </summary>
public interface IResultRepository
{
    /// <summary>
    /// Добавляет новую запись результата либо обновляет существующую, если запись
    /// с таким же именем файла уже присутствует в базе.
    /// </summary>
    /// <param name="result">Агрегированный результат, подлежащий сохранению.</param>
    Task UpsertAsync(ResultRecord result);

    /// <summary>
    /// Возвращает список результатов, удовлетворяющих переданным условиям фильтрации.
    /// </summary>
    /// <param name="filter">Набор условий фильтрации (по имени файла, диапазонам дат и показателей).</param>
    /// <returns>Список результатов, прошедших фильтрацию.</returns>
    Task<IReadOnlyList<ResultRecord>> GetFilteredAsync(ResultFilter filter);
}
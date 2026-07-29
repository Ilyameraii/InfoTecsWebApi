using Entities;
using UseCases.Contracts.Models;

namespace UseCases.Contracts;

/// <summary>
/// Use case получения списка агрегированных результатов из таблицы Results,
/// подходящих под заданные фильтры (имя файла, диапазон времени запуска,
/// диапазон среднего показателя, диапазон среднего времени выполнения).
/// </summary>
public interface IGetFilteredResultsUseCase
{
    /// <summary>
    /// Возвращает записи результатов, удовлетворяющие переданным фильтрам.
    /// Фильтры не заданы — применяются только те, значения которых указаны.
    /// </summary>
    /// <param name="filters">Параметры фильтрации.</param>
    /// <returns>Список отфильтрованных записей <see cref="ResultRecord"/>.</returns>
    Task<IReadOnlyList<ResultRecord>> ExecuteAsync(ResultFilterRequest filters);
}
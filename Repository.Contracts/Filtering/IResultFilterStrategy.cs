using Entities;
using Repository.Contracts.Models;

namespace Repository.Contracts.Filtering;

/// <summary>
/// Стратегия фильтрации результатов (таблица Results) по одному из полей
/// <see cref="ResultFilter"/>. Каждая реализация отвечает за один критерий
/// фильтрации и подключается в <see cref="ResultRepository"/> через DI
/// как элемент коллекции стратегий.
/// </summary>
public interface IResultFilterStrategy
{
    /// <summary>
    /// Определяет, задан ли в переданном фильтре критерий, за который
    /// отвечает данная стратегия.
    /// </summary>
    /// <param name="filter">Фильтр, полученный от пользователя.</param>
    /// <returns><c>true</c>, если критерий стратегии задан и должен быть применён; иначе <c>false</c>.</returns>
    bool IsApplicable(ResultFilter filter);
    
    /// <summary>
    /// Применяет условие фильтрации к запросу.
    /// </summary>
    /// <param name="query">Исходный запрос к таблице Results.</param>
    /// <param name="filter">Фильтр, содержащий значения для условия.</param>
    /// <returns>Запрос с добавленным условием фильтрации.</returns> 
    IQueryable<ResultRecord> Apply(IQueryable<ResultRecord> query, ResultFilter filter);
}
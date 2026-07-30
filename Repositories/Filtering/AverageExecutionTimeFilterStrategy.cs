using Entities;
using Repository.Contracts.Filtering;
using Repository.Contracts.Models;

namespace Repositories.Filtering;

/// <summary>
/// Стратегия фильтрации результатов по диапазону среднего времени выполнения
/// (AverageExecutionTime).
/// </summary>
public class AverageExecutionTimeFilterStrategy : IResultFilterStrategy
{
    /// <inheritdoc/>
    public bool IsApplicable(ResultFilter filter) =>
        filter.AverageExecutionTimeFrom.HasValue || filter.AverageExecutionTimeTo.HasValue;

    /// <inheritdoc/>
    public IQueryable<ResultRecord> Apply(IQueryable<ResultRecord> query, ResultFilter filter)
    {
        if (filter.AverageExecutionTimeFrom.HasValue)
        {
            query = query.Where(r => r.AverageExecutionTime >= filter.AverageExecutionTimeFrom.Value);
        }

        if (filter.AverageExecutionTimeTo.HasValue)
        {
            query = query.Where(r => r.AverageExecutionTime <= filter.AverageExecutionTimeTo.Value);
        }

        return query;
    }
}
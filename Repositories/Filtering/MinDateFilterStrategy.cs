using Entities;
using Repository.Contracts.Filtering;
using Repository.Contracts.Models;

namespace Repositories.Filtering;

/// <summary>
/// Стратегия фильтрации результатов по диапазону времени запуска первой
/// операции (MinDate).
/// </summary>
public class MinDateFilterStrategy : IResultFilterStrategy
{
    /// <inheritdoc/>
    public bool IsApplicable(ResultFilter filter) =>
        filter.MinDateFrom.HasValue || filter.MinDateTo.HasValue;

    /// <inheritdoc/>
    public IQueryable<ResultRecord> Apply(IQueryable<ResultRecord> query, ResultFilter filter)
    {
        if (filter.MinDateFrom.HasValue)
        {
            query = query.Where(r => r.MinDate >= filter.MinDateFrom.Value);
        }

        if (filter.MinDateTo.HasValue)
        {
            query = query.Where(r => r.MinDate <= filter.MinDateTo.Value);
        }

        return query;
    }
}
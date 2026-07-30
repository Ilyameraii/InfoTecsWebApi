using Entities;
using Repository.Contracts.Filtering;
using Repository.Contracts.Models;

namespace Repositories.Filtering;

/// <summary>
/// Стратегия фильтрации результатов по диапазону среднего значения показателя
/// (AverageValue).
/// </summary>
public class AverageValueFilterStrategy : IResultFilterStrategy
{
    /// <inheritdoc/>
    public bool IsApplicable(ResultFilter filter) =>
        filter.AverageValueFrom.HasValue || filter.AverageValueTo.HasValue;

    /// <inheritdoc/>
    public IQueryable<ResultRecord> Apply(IQueryable<ResultRecord> query, ResultFilter filter)
    {
        if (filter.AverageValueFrom.HasValue)
            query = query.Where(r => r.AverageValue >= filter.AverageValueFrom.Value);

        if (filter.AverageValueTo.HasValue)
            query = query.Where(r => r.AverageValue <= filter.AverageValueTo.Value);

        return query;
    }
}
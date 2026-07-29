using Entities;
using Repository.Contracts.Filtering;
using Repository.Contracts.Models;

namespace Repositories.Filtering;

public class AverageValueFilterStrategy : IResultFilterStrategy
{
    public bool IsApplicable(ResultFilter filter) =>
        filter.AverageValueFrom.HasValue || filter.AverageValueTo.HasValue;

    public IQueryable<ResultRecord> Apply(IQueryable<ResultRecord> query, ResultFilter filter)
    {
        if (filter.AverageValueFrom.HasValue)
            query = query.Where(r => r.AverageValue >= filter.AverageValueFrom.Value);

        if (filter.AverageValueTo.HasValue)
            query = query.Where(r => r.AverageValue <= filter.AverageValueTo.Value);

        return query;
    }
}
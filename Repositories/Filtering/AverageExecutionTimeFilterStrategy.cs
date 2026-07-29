using Entities;
using Repository.Contracts.Filtering;
using Repository.Contracts.Models;

namespace Repositories.Filtering;

public class AverageExecutionTimeFilterStrategy : IResultFilterStrategy
{
    public bool IsApplicable(ResultFilter filter) =>
        filter.AverageExecutionTimeFrom.HasValue || filter.AverageExecutionTimeTo.HasValue;

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
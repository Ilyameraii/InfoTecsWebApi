using Entities;
using Repository.Contracts.Filtering;
using Repository.Contracts.Models;

namespace Repositories.Filtering;

public class FileNameFilterStrategy : IResultFilterStrategy
{
    public bool IsApplicable(ResultFilter filter) => !string.IsNullOrWhiteSpace(filter.FileName);

    public IQueryable<ResultRecord> Apply(IQueryable<ResultRecord> query, ResultFilter filter)
    {
        return query.Where(r => r.FileName == filter.FileName);
    }
}
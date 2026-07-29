using Entities;
using Repository.Contracts.Models;

namespace Repository.Contracts.Filtering;

public interface IResultFilterStrategy
{
    bool IsApplicable(ResultFilter filter);
    IQueryable<ResultRecord> Apply(IQueryable<ResultRecord> query, ResultFilter filter);
}
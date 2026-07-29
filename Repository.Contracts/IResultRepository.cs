using Entities;
using Repository.Contracts.Models;

namespace Repository.Contracts;

public interface IResultRepository
{
    Task UpsertAsync(ResultRecord result);

    Task<IReadOnlyList<ResultRecord>> GetFilteredAsync(ResultFilter filter);
}

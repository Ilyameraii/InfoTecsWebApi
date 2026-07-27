using Entities;
using Repository.Contracts.Models;

namespace Repository.Contracts;

public interface IResultRepository
{
    Task UpsertAsync(ResultRecord result);

    Task<List<ResultRecord>> GetFilteredAsync(ResultFilter filter);
}

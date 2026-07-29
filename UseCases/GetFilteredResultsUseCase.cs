using Entities;
using Repository.Contracts;
using Repository.Contracts.Models;
using UseCases.Contracts;

namespace UseCases;

public class GetFilteredResultsUseCase(IResultRepository resultRepository):IGetFilteredResultsUseCase
{
    public async Task<IReadOnlyList<ResultRecord>> ExecuteAsync(ResultFilter filters)
    {
        return await resultRepository.GetFilteredAsync(filters);
    }
}
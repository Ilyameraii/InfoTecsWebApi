using Entities;
using Repository.Contracts.Models;

namespace UseCases.Contracts;

public interface IGetFilteredResultsUseCase
{
    Task<IReadOnlyList<ResultRecord>> ExecuteAsync(ResultFilter filters);
}
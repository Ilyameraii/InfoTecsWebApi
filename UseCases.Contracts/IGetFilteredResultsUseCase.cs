using Repository.Contracts.Models;

namespace UseCases.Contracts;

public interface IGetFilteredResultsUseCase
{
    Task ExecuteAsync(ResultFilter filters);
}
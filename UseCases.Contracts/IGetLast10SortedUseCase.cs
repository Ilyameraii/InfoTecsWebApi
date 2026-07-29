using Entities;

namespace UseCases.Contracts;

public interface IGetLast10SortedUseCase
{
    Task<IReadOnlyList<ValueRecord>> ExecuteAsync(string fileName);
}
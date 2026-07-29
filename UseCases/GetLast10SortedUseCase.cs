using Entities;
using Repository.Contracts;
using UseCases.Contracts;

namespace UseCases;

public class GetLast10SortedUseCase(IValueRepository repository):IGetLast10SortedUseCase
{
    public async Task<IReadOnlyList<ValueRecord>> ExecuteAsync(string fileName)
    {
        return await repository.GetLast10SortedAsync(fileName);
    }
}
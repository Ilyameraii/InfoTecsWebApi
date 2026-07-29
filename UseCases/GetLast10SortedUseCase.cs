using Entities;
using Repository.Contracts;
using UseCases.Contracts;

namespace UseCases;

/// <summary>
/// Реализация <see cref="IGetLast10SortedUseCase"/>.
/// </summary>
public class GetLast10SortedUseCase(IValueRepository repository):IGetLast10SortedUseCase
{
    /// <inheritdoc/>
    public async Task<IReadOnlyList<ValueRecord>> ExecuteAsync(string fileName)
    {
        return await repository.GetLast10SortedAsync(fileName);
    }
}
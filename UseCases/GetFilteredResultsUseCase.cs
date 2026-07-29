using Entities;
using Repository.Contracts;
using Repository.Contracts.Models;
using UseCases.Contracts;
using UseCases.Contracts.Models;

namespace UseCases;

/// <summary>
/// Реализация <see cref="IGetFilteredResultsUseCase"/>.
/// </summary>
public class GetFilteredResultsUseCase(IResultRepository resultRepository):IGetFilteredResultsUseCase
{
    /// <inheritdoc/>
    public async Task<IReadOnlyList<ResultRecord>> ExecuteAsync(ResultFilterRequest filters)
    {
        var filter = new ResultFilter
        {
            FileName = filters.FileName,
            MinDateFrom = filters.MinDateFrom,
            MinDateTo = filters.MinDateTo,
            AverageValueFrom = filters.AverageValueFrom,
            AverageValueTo = filters.AverageValueTo,
            AverageExecutionTimeFrom = filters.AverageExecutionTimeFrom,
            AverageExecutionTimeTo = filters.AverageExecutionTimeTo
        };
        
        return await resultRepository.GetFilteredAsync(filter);
    }
}
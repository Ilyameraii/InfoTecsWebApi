using Context;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Repository.Contracts.Filtering;
using Repository.Contracts.Models;

namespace Repositories;


public class ResultRepository(AppDbContext context, IEnumerable<IResultFilterStrategy> strategies) : IResultRepository
{
    public async Task UpsertAsync(ResultRecord result)
    {
        var existing = await GetByFileNameAsync(result.FileName);

        if (existing is null)
        {
            await context.Results.AddAsync(result);
            return;
        }

        existing.DeltaSeconds = result.DeltaSeconds;
        existing.MinDate = result.MinDate;
        existing.AverageExecutionTime = result.AverageExecutionTime;
        existing.AverageValue = result.AverageValue;
        existing.MedianValue = result.MedianValue;
        existing.MaxValue = result.MaxValue;
        existing.MinValue = result.MinValue;
    }
    private async Task<ResultRecord?> GetByFileNameAsync(string fileName)
    {
        return await context.Results
            .FirstOrDefaultAsync(r => r.FileName == fileName);
    }
    
    public async Task<IReadOnlyList<ResultRecord>> GetFilteredAsync(ResultFilter filter)
    {
        var query = context.Results.AsQueryable();

        foreach (var strategy in strategies.Where(s => s.IsApplicable(filter)))
        {
            query = strategy.Apply(query, filter);
        }

        return await query.ToListAsync();
    }
}
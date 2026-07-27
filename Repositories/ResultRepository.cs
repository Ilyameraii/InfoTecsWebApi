using Context;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Repository.Contracts.Models;

namespace Repositories;


public class ResultRepository(AppDbContext db) : IResultRepository
{
    public async Task UpsertAsync(ResultRecord result)
    {
        var existing = await GetByFileNameAsync(result.FileName);

        if (existing is null)
        {
            await db.Results.AddAsync(result);
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
        return await db.Results
            .FirstOrDefaultAsync(r => r.FileName == fileName);
    }
    
    public async Task<List<ResultRecord>> GetFilteredAsync(ResultFilter filter)
    {
        var query = db.Results.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.FileName))
        {
            query = query.Where(r => r.FileName == filter.FileName);
        }

        if (filter.MinDateFrom.HasValue)
        {
            query = query.Where(r => r.MinDate >= filter.MinDateFrom.Value);
        }

        if (filter.MinDateTo.HasValue)
        {
            query = query.Where(r => r.MinDate <= filter.MinDateTo.Value);
        }

        if (filter.AverageValueFrom.HasValue)
        {
            query = query.Where(r => r.AverageValue >= filter.AverageValueFrom.Value);
        }

        if (filter.AverageValueTo.HasValue)
        {
            query = query.Where(r => r.AverageValue <= filter.AverageValueTo.Value);
        }

        if (filter.AverageExecutionTimeFrom.HasValue)
        {
            query = query.Where(r => r.AverageExecutionTime >= filter.AverageExecutionTimeFrom.Value);
        }

        if (filter.AverageExecutionTimeTo.HasValue)
        {
            query = query.Where(r => r.AverageExecutionTime <= filter.AverageExecutionTimeTo.Value);
        }

        return await query.AsNoTracking().ToListAsync();
    }
}
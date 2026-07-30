using Context;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repositories;

/// <summary>
/// Реализация <see cref="IValueRepository"/> на основе EF Core и <see cref="AppDbContext"/>.
/// Обеспечивает удаление, массовую вставку и выборку последних значений
/// для таблицы Values.
/// </summary>
public class ValueRepository(AppDbContext context) : IValueRepository
{
    /// <inheritdoc/>
    public async Task DeleteByFileNameAsync(string fileName)
    {
        await context.Values
            .Where(v => v.FileName == fileName)
            .ExecuteDeleteAsync();
    }

    /// <inheritdoc/>
    public async Task AddRangeAsync(IEnumerable<ValueRecord> values)
    {
        await context.Values.AddRangeAsync(values);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<ValueRecord>> GetLast10SortedAsync(string fileName)
    {
        return await context.Values.AsNoTracking()
            .Where(v => v.FileName == fileName)
            .OrderByDescending(v => v.Date)
            .Take(10)
            .ToListAsync();
    }
}
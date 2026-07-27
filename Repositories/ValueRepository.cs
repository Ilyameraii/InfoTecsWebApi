using Context;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repositories;

public class ValueRepository(AppDbContext context) : IValueRepository
{
    public async Task DeleteByFileNameAsync(string fileName)
    {
        await context.Values
            .Where(v => v.FileName == fileName)
            .ExecuteDeleteAsync();
    }

    public async Task AddRangeAsync(IEnumerable<ValueRecord> values)
    {
        await context.Values.AddRangeAsync(values);
    }
}
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Services.Contracts;

namespace Context;

public class UnitOfWork(
    AppDbContext context,
    IValueRepository valueRepository,
    IResultRepository resultRepository) : IUnitOfWork
{
    public IValueRepository Values => valueRepository;
    public IResultRepository Results => resultRepository;

    public async Task ExecuteInTransactionAsync(Func<Task> action)
    {
        var strategy = context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await action();
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        });
    }
}
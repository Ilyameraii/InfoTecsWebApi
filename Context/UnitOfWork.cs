using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Services.Contracts;

namespace Context;

public class UnitOfWork(
    AppDbContext dbContext,
    IValueRepository valueRepository,
    IResultRepository resultRepository) : IUnitOfWork
{
    public IValueRepository Values => valueRepository;
    public IResultRepository Results => resultRepository;

    public async Task ExecuteInTransactionAsync(Func<Task> action)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                await action();
                await dbContext.SaveChangesAsync();
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
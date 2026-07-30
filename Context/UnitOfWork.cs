using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Services.Contracts;

namespace Context;

/// <summary>
/// Реализация <see cref="IUnitOfWork"/>.
/// Предоставляет доступ к репозиториям <see cref="IValueRepository"/> и <see cref="IResultRepository"/>
/// и оборачивает выполнение операций в транзакцию базы данных с автоматическим коммитом
/// или откатом при возникновении ошибки, используя стратегию выполнения EF Core.
/// </summary>
public class UnitOfWork(
    AppDbContext context,
    IValueRepository valueRepository,
    IResultRepository resultRepository) : IUnitOfWork
{
    /// <inheritdoc/>
    public IValueRepository Values => valueRepository;
    
    /// <inheritdoc/>
    public IResultRepository Results => resultRepository;

    /// <inheritdoc/>
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
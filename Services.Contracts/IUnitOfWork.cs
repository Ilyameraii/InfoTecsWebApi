using Repository.Contracts;

namespace Services.Contracts;

public interface IUnitOfWork
{
    IValueRepository Values { get; }
    IResultRepository Results { get; }

    Task ExecuteInTransactionAsync(Func<Task> action);
}
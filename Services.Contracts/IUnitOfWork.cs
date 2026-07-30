using Repository.Contracts;

namespace Services.Contracts;

/// <summary>
/// Предоставляет единую точку доступа к репозиториям и обеспечивает атомарность
/// операций записи в базу данных в рамках одной транзакции (паттерн Unit of Work).
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Репозиторий для работы с исходными значениями (таблица Values).
    /// </summary>
    IValueRepository Values { get; }

    /// <summary>
    /// Репозиторий для работы с агрегированными результатами (таблица Results).
    /// </summary>
    IResultRepository Results { get; }

    /// <summary>
    /// Выполняет переданное действие в рамках одной транзакции базы данных
    /// и сохраняет изменения при успешном завершении.
    /// </summary>
    /// <param name="action">Асинхронное действие, выполняющее операции над репозиториями.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    Task ExecuteInTransactionAsync(Func<Task> action);
}
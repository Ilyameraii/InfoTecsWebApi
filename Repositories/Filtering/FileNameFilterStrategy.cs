using Entities;
using Repository.Contracts.Filtering;
using Repository.Contracts.Models;

namespace Repositories.Filtering;

/// <summary>
/// Стратегия фильтрации результатов по точному совпадению имени файла
/// (FileName).
/// </summary>
public class FileNameFilterStrategy : IResultFilterStrategy
{
    /// <inheritdoc/>
    public bool IsApplicable(ResultFilter filter) => !string.IsNullOrWhiteSpace(filter.FileName);

    /// <inheritdoc/>
    public IQueryable<ResultRecord> Apply(IQueryable<ResultRecord> query, ResultFilter filter)
    {
        return query.Where(r => r.FileName == filter.FileName);
    }
}
using Entities;

namespace Repository.Contracts;

public interface IValueRepository
{
    Task DeleteByFileNameAsync(string fileName);

    Task AddRangeAsync(IEnumerable<ValueRecord> values);

    Task<IReadOnlyList<ValueRecord>> GetLast10SortedAsync(string fileName);
}
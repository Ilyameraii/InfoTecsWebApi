using Entities;

namespace Services.Contracts;

public interface ICsvParser
{
    Task<IReadOnlyCollection<ValueRecord>> ParseAsync(Stream csvStream, string fileName);
}
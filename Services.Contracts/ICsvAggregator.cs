using Entities;

namespace Services.Contracts;

public interface ICsvAggregator
{
    ResultRecord Calculate(string fileName, IReadOnlyCollection<ValueRecord> rows);
}
using Entities;

namespace Services.Contracts.Validation;

public interface ICsvValidator
{
    void Validate(IReadOnlyCollection<ValueRecord> rows);
}
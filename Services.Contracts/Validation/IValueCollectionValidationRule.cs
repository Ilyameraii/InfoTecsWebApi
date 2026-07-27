using Entities;

namespace Services.Contracts.Validation;

public interface IValueCollectionValidationRule
{
    bool IsValid(IReadOnlyCollection<ValueRecord> items, out string? error);
}
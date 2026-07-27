using Entities;

namespace Services.Contracts.Validation;

public interface IValueValidationRule
{
    bool IsValid(ValueRecord item, out string? error);
}
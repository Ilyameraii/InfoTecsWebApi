namespace Services.Contracts.Exceptions;

public class CsvValidationException(IReadOnlyList<string> errors)
    : Exception($"CSV содержит ошибки: {string.Join("; ", errors)}")
{
    public IReadOnlyList<string> Errors { get; } = errors;

    public CsvValidationException(string error)
        : this([error])
    {
    }
}
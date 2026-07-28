namespace UseCases.Contracts;

public interface ICsvFileProcessingUseCase
{
    Task ExecuteAsync(Stream stream, string fileFileName);
}
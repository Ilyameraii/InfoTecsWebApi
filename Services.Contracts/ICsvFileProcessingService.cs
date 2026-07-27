namespace Services.Contracts;

public interface ICsvFileProcessingService
{
    Task ProcessAsync(Stream stream, string fileFileName);
}
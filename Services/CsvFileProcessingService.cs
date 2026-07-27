using Services.Contracts;
using Services.Contracts.Validation;

namespace Services;

public class CsvFileProcessingService(
    ICsvParser csvParser,
    ICsvValidator csvValidator,
    ICsvAggregator csvAggregator,
    IUnitOfWork unitOfWork) : ICsvFileProcessingService
{

    public async Task ProcessAsync(Stream csvStream, string fileName)
    {
        var values = await csvParser.ParseAsync(csvStream,fileName);
        
        csvValidator.Validate(values);
        
        
        var result = csvAggregator.Calculate(fileName, values);

        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await unitOfWork.Values.DeleteByFileNameAsync(fileName);
            await unitOfWork.Values.AddRangeAsync(values);
            await unitOfWork.Results.UpsertAsync(result);
        });
    }
    
}
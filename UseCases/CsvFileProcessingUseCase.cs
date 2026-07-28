using Services.Contracts;
using Services.Contracts.Validation;
using UseCases.Contracts;

namespace UseCases;

public class CsvFileProcessingUseCase(
    ICsvParser csvParser,
    ICsvValidator csvValidator,
    ICsvAggregator csvAggregator,
    IUnitOfWork unitOfWork) : ICsvFileProcessingUseCase
{

    public async Task ExecuteAsync(Stream csvStream, string fileName)
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
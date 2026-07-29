using Services.Contracts;
using Services.Contracts.Validation;
using UseCases.Contracts;

namespace UseCases;

/// <summary>
/// Реализация <see cref="ICsvFileProcessingUseCase"/>.
/// Оркестрирует полный цикл обработки файла: парсинг (<see cref="ICsvParser"/>),
/// валидацию (<see cref="ICsvValidator"/>), расчёт агрегатов (<see cref="ICsvAggregator"/>)
/// и атомарное сохранение строк и результата через <see cref="IUnitOfWork"/>.
/// </summary>
public class CsvFileProcessingUseCase(
    ICsvParser csvParser,
    ICsvValidator csvValidator,
    ICsvAggregator csvAggregator,
    IUnitOfWork unitOfWork) : ICsvFileProcessingUseCase
{
    /// <inheritdoc/>
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
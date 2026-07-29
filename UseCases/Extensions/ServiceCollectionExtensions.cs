using Microsoft.Extensions.DependencyInjection;
using UseCases.Contracts;

namespace UseCases.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<ICsvFileProcessingUseCase, CsvFileProcessingUseCase>();
        services.AddScoped<IGetFilteredResultsUseCase, GetFilteredResultsUseCase>();
        services.AddScoped<IGetLast10SortedUseCase, GetLast10SortedUseCase>();
        
        return services;
    }
}
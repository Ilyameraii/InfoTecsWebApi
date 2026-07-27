using Services.Contracts;
using Services.Contracts.Validation;
using Services.Validation;
using Services.Validation.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCsvValidation(this IServiceCollection services)
    {
        services.AddSingleton<IValueValidationRule, DateNotInFutureRule>();
        services.AddSingleton<IValueValidationRule, DateNotBeforeMinRule>();
        services.AddSingleton<IValueValidationRule, ExecutionTimeNonNegativeRule>();
        services.AddSingleton<IValueValidationRule, ValueNonNegativeRule>();

        services.AddSingleton<IValueCollectionValidationRule, RowCountRule>();

        services.AddSingleton<CsvValidator>();

        return services;
    }

    public static IServiceCollection AddValuesServices(this IServiceCollection services)
    {
        services.AddCsvValidation();

        services.AddScoped<ICsvFileProcessingService, CsvFileProcessingService>();

        return services;
    }
}
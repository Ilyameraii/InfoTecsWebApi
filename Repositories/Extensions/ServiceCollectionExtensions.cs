using Microsoft.Extensions.DependencyInjection;
using Repositories.Filtering;
using Repository.Contracts;
using Repository.Contracts.Filtering;

namespace Repositories.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    { 
       services.AddScoped<IResultRepository, ResultRepository>();
       services.AddScoped<IValueRepository, ValueRepository>();

       return services;
    }
    
    public static IServiceCollection AddFilterStrategies(this IServiceCollection services)
    {
        services.AddSingleton<IResultFilterStrategy, FileNameFilterStrategy>();
        services.AddSingleton<IResultFilterStrategy, MinDateFilterStrategy>();
        services.AddSingleton<IResultFilterStrategy, AverageValueFilterStrategy>();
        services.AddSingleton<IResultFilterStrategy, AverageExecutionTimeFilterStrategy>();
        
        return services;
    }
}
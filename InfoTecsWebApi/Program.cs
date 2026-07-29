using Context;
using InfoTecsWebApi.Middleware;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Extensions;
using Repository.Contracts;
using Services;
using Services.Contracts;
using Services.Extensions;
using UseCases;
using UseCases.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(); // генератор OpenAPI-документа от Swashbuckle

// --- DbContext (Npgsql) ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRepositories();
builder.Services.AddFilterStrategies();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton<ICsvParser, CsvParser>();
builder.Services.AddSingleton<ICsvAggregator, CsvAggregator>();

builder.Services.AddScoped<ICsvFileProcessingUseCase, CsvFileProcessingUseCase>();
builder.Services.AddScoped<IGetFilteredResultsUseCase, GetFilteredResultsUseCase>();

builder.Services.AddCsvValidation();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>(); // вот тут, до MapControllers и т.д.

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();       // публикует JSON, обычно по /swagger/v1/swagger.json
    app.UseSwaggerUI();     // рисует UI, обычно по /swagger
}

app.UseHttpsRedirection();

app.Run();

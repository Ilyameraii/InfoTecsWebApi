using Context;
using InfoTecsWebApi.Middleware;
using Microsoft.EntityFrameworkCore;
using Repositories.Extensions;
using Services;
using Services.Contracts;
using Services.Extensions;
using UseCases;
using UseCases.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

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
builder.Services.AddScoped<IGetLast10SortedUseCase, GetLast10SortedUseCase>();

builder.Services.AddCsvValidation();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();      
    app.UseSwaggerUI();     
}

app.UseHttpsRedirection();

app.Run();

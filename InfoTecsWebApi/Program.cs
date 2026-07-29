using Context;
using InfoTecsWebApi.Middleware;
using Microsoft.EntityFrameworkCore;
using Repositories.Extensions;
using Services;
using Services.Contracts;
using Services.Extensions;
using UseCases.Extensions;

var builder = WebApplication.CreateBuilder(args);

// API
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// База данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Репозитории / Unit of Work
builder.Services.AddRepositories();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddFilterStrategies();

// CSV-сервисы
builder.Services.AddSingleton<ICsvParser, CsvParser>();
builder.Services.AddSingleton<ICsvAggregator, CsvAggregator>();
builder.Services.AddCsvValidation();

// Use cases
builder.Services.AddUseCases();

var app = builder.Build();

// Middleware pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
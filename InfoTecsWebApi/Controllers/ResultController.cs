using Microsoft.AspNetCore.Mvc;
using UseCases.Contracts;
using UseCases.Contracts.Models;

namespace InfoTecsWebApi.Controllers;

/// <summary>
/// Контроллер для получения агрегированных результатов обработки CSV-файлов
/// из таблицы Results с применением фильтров.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ResultController(IGetFilteredResultsUseCase useCase): ControllerBase
{
    /// <summary>
    /// Возвращает список записей результатов, подходящих под переданные фильтры
    /// (имя файла, диапазон времени запуска, диапазон среднего показателя,
    /// диапазон среднего времени выполнения).
    /// </summary>
    /// <param name="filters">Параметры фильтрации, переданные через query-строку.</param>
    /// <returns>Список отфильтрованных записей <see cref="Entities.ResultRecord"/>.</returns>
    [HttpGet]
    public async Task<IActionResult> GetFiltered([FromQuery] ResultFilterRequest filters)
    { 
        var results = await useCase.ExecuteAsync(filters);
        return Ok(results);
    }
}
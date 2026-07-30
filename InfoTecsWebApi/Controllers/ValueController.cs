using Microsoft.AspNetCore.Mvc;
using UseCases.Contracts;

namespace InfoTecsWebApi.Controllers;

/// <summary>
/// Контроллер для загрузки CSV-файлов с результатами обработки
/// и получения последних значений по конкретному файлу.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ValueController(
    ICsvFileProcessingUseCase csvFileProcessingUseCase,
    IGetLast10SortedUseCase getLast10SortedUseCase) : ControllerBase
{
    /// <summary>
    /// Принимает CSV-файл, валидирует и сохраняет его строки в таблицу Values,
    /// пересчитывает и сохраняет агрегированные результаты в таблицу Results.
    /// Если файл с таким именем уже существует, его данные перезаписываются.
    /// </summary>
    /// <param name="file">Загружаемый CSV-файл.</param>
    /// <returns>200 OK при успешной обработке; 400 Bad Request со списком ошибок при невалидном файле.</returns>
    [HttpPost("upload")]
    public async Task<IActionResult> UploadCsv(IFormFile? file)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest("Файл не выбран или пуст");
        }

        await using var stream = file.OpenReadStream();
        await csvFileProcessingUseCase.ExecuteAsync(stream, file.FileName);

        return Ok();
    }

    /// <summary>
    /// Возвращает последние 10 значений по указанному файлу,
    /// отсортированные по времени запуска (Date).
    /// </summary>
    /// <param name="fileName">Имя файла, по которому нужно получить значения.</param>
    /// <returns>Список последних 10 записей <see cref="Entities.ValueRecord"/>.</returns>
    [HttpGet("last10")]
    public async Task<IActionResult> Last10(string fileName)
    {
        var result = await getLast10SortedUseCase.ExecuteAsync(fileName);

        return Ok(result);
    }
}
using Microsoft.AspNetCore.Mvc;
using UseCases.Contracts;

namespace InfoTecsWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ValueController(
    ICsvFileProcessingUseCase csvFileProcessingUseCase,
    IGetLast10SortedUseCase getLast10SortedUseCase ) : ControllerBase
{
    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadCsv(IFormFile file)
    {
        if (file.Length == 0)
        {
            return BadRequest("Файл пуст");
        }

        await using var stream = file.OpenReadStream();
        await csvFileProcessingUseCase.ExecuteAsync(stream, file.FileName);

        return Ok();
    }

    [HttpGet("last10")]
    public async Task<IActionResult> Last10(string fileName)
    {
        var result = await getLast10SortedUseCase.ExecuteAsync(fileName);

        return Ok(result);
    }
}
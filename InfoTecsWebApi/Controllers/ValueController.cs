using Microsoft.AspNetCore.Mvc;
using UseCases.Contracts;

namespace InfoTecsWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ValueController(ICsvFileProcessingUseCase useCase) : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("upload")]
    public async Task<IActionResult> UploadCsv(IFormFile file)
    {
        if (file.Length == 0)
        {
            return BadRequest("Файл пуст");
        }

        await using var stream = file.OpenReadStream();
        await useCase.ExecuteAsync(stream, file.FileName);

        return Ok();
    }
}
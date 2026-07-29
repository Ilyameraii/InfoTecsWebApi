using Microsoft.AspNetCore.Mvc;
using UseCases.Contracts;

namespace InfoTecsWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ValueController(ICsvFileProcessingUseCase fileProcessingUseCase) : ControllerBase
{
    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadCsv(IFormFile file)
    {
        if (file.Length == 0)
        {
            return BadRequest("Файл пуст");
        }

        await using var stream = file.OpenReadStream();
        await fileProcessingUseCase.ExecuteAsync(stream, file.FileName);

        return Ok();
    }
}
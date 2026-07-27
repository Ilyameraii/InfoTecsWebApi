using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace InfoTecsWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ValueController(ICsvFileProcessingService csvFileProcessingService) : ControllerBase
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
        await csvFileProcessingService.ProcessAsync(stream, file.FileName);

        return Ok();
    }
}
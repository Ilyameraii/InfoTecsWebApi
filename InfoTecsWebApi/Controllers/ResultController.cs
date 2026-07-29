using Microsoft.AspNetCore.Mvc;
using Repository.Contracts.Models;
using UseCases.Contracts;

namespace InfoTecsWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ResultController(IGetFilteredResultsUseCase useCase): ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetFiltered([FromQuery] ResultFilter filters)
    { 
        var results = await useCase.ExecuteAsync(filters);
        return Ok(results);
    }
}
using ColorRegressionApp.DTO;
using ColorRegressionApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ColorRegressionApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModelController : ControllerBase
{
    private readonly IRegressionService _regressionService;

    public ModelController(IRegressionService regressionService)
    {
        _regressionService = regressionService;
    }

    [HttpPost("Build")]
    public async Task<IActionResult> Build([FromBody] BuildModelRequest request)
    {
        var result = await _regressionService.BuildModelAsync(request);
        return Ok(result);
    }

    [HttpGet("Latest")]
    public async Task<IActionResult> Latest()
    {
        var result = await _regressionService.GetLatestModelAsync();
        if (result == null)
            return NotFound("Модели не найдены.");

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _regressionService.GetModelAsync(id);
        if (result == null)
            return NotFound("Модель не найдена.");

        return Ok(result);
    }

    [HttpGet("List")]
    public async Task<IActionResult> List()
    {
        var result = await _regressionService.GetModelsAsync();
        return Ok(result);
    }
}
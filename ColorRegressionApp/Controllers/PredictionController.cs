using ColorRegressionApp.DTO;
using ColorRegressionApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ColorRegressionApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PredictionController : ControllerBase
{
    private readonly IPredictionService _predictionService;

    public PredictionController(IPredictionService predictionService)
    {
        _predictionService = predictionService;
    }

    [HttpPost("Calculate")]
    public async Task<IActionResult> Calculate([FromBody] PredictionRequest request)
    {
        try
        {
            var result = await _predictionService.CalculateAsync(request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
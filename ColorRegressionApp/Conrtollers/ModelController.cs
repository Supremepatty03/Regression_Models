[ApiController]
[Route("api/[controller]")]

public class ModelController : ControllerBase
{
    private readonly IRegressionService _regressionService;

    public ModelController(IRegressionService regressionService)
    {
        _regressionService = regressionService;
    }

    [HttpPost("BuildModel")]
    public IActionResult BuildModel([FromBody] BuildModelRequest request)
    {
        var response = _regressionService.BuildModel(request);
        return Ok(response);
    }
}
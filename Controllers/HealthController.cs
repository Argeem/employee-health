using Models.Entities;
using Services;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly IHealthService _healthService;
    
    public HealthController(IHealthService healthService)
    {
        _healthService = healthService;
    }

    [HttpGet("summary/{empId}")]
    public async Task<IActionResult> GetSummary(string empId)
    {
        var vm = await _healthService.GetHealthSummaryAsync(empId);
        return Ok(vm);
    }

    [HttpPost("log")]
    public async Task<IActionResult> LogExercise([FromBody] ExerciseLog input)
    {
        await _healthService.LogExerciseAsync(input);
        return Accepted();
    }
}

using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Entities;
using Services;

namespace EmployeeHealth.Web.Pages;

public class ExerciseLogsModel : PageModel
{
    private readonly IHealthService _healthService;

    public ExerciseLogsModel(IHealthService healthService)
    {
        _healthService = healthService;
    }

    public List<ExerciseLog> ExerciseLogs { get; set; } = new();
    public int TotalLogs { get; set; }
    public int TotalMinutes { get; set; }
    public int AverageMinutes { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            ExerciseLogs = await _healthService.GetExerciseLogsAsync("EMP001");
            
            TotalLogs = ExerciseLogs.Count;
            TotalMinutes = ExerciseLogs.Sum(l => l.DurationMin);
            AverageMinutes = TotalLogs > 0 ? TotalMinutes / TotalLogs : 0;
        }
        catch (Exception ex)
        {
            // Log error and continue with empty list
            ExerciseLogs = new();
        }
    }
}

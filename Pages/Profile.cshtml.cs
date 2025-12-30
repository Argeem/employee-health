using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Models.ViewModels;
using Services;

namespace EmployeeHealth.Web.Pages;

public class ProfileModel : PageModel
{
    private readonly IHealthService _healthService;
    private readonly ILogger<ProfileModel> _logger;

    public ProfileModel(IHealthService healthService, ILogger<ProfileModel> logger)
    {
        _healthService = healthService;
        _logger = logger;
    }

    public HealthSummaryVm? HealthData { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            // Example employee ID (you can replace with dynamic value from session/claims)
            string empId = "EMP001";

            _logger.LogInformation("Fetching health data for employee: {EmployeeId}", empId);

            HealthData = await _healthService.GetHealthSummaryAsync(empId);

            if (HealthData != null)
            {
                _logger.LogInformation("Successfully retrieved health data from database - EmpId: {EmpId}, Name: {Name}, LatestWeight: {LatestWeight}, ExerciseCount: {ExerciseCount}", 
                    HealthData.EmpId, HealthData.Name, HealthData.LatestWeight, HealthData.ExerciseCount);
            }
            else
            {
                _logger.LogWarning("No health data found for employee: {EmployeeId}", empId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching health data from database");
        }
    }
}

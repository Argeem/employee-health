using Models.Entities;
using Models.ViewModels;
using Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeHealth.Web.Pages;

public class DashboardModel : PageModel
{
    private readonly IHealthService _healthService;

    public DashboardModel(IHealthService healthService)
    {
        _healthService = healthService;
    }

    [BindProperty]
    public ExerciseLog Input { get; set; } = new();

    public HealthSummaryVm Summary { get; set; } = new();

    public async Task OnGetAsync()
    {
        Summary = await _healthService.GetHealthSummaryAsync("EMP-001");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // map posted fields to ExerciseLog
        if (!ModelState.IsValid)
        {
            Summary = await _healthService.GetHealthSummaryAsync("EMP-001");
            return Page();
        }

        Input.EmpId = "EMP-001";
        await _healthService.LogExerciseAsync(Input);

        return RedirectToPage();
    }
}

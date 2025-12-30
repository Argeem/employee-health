using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.ViewModels;
using Services;

namespace EmployeeHealth.Web.Pages;

public class KpiResultsModel : PageModel
{
    private readonly IHealthService _healthService;

    public KpiResultsModel(IHealthService healthService)
    {
        _healthService = healthService;
    }

    public HealthSummaryVm Summary { get; set; } = new();
    public List<MonthlyKpiVm> MonthlyKpis { get; set; } = new();

    public async Task OnGetAsync()
    {
        // Fetch current employee data (EMP001)
        Summary = await _healthService.GetHealthSummaryAsync("EMP001");
        
        // Fetch monthly KPI data
        MonthlyKpis = await _healthService.GetMonthlyKpisAsync("EMP001");
    }
}

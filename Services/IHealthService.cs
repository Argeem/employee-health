using Models.Entities;
using Models.ViewModels;

namespace Services;

public interface IHealthService
{
    Task<HealthSummaryVm> GetHealthSummaryAsync(string empId);
    Task<List<ExerciseLog>> GetExerciseLogsAsync(string empId);
    Task<List<MonthlyKpiVm>> GetMonthlyKpisAsync(string empId);
    Task LogExerciseAsync(ExerciseLog input);
}

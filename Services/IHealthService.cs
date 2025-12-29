using Models.Entities;
using Models.ViewModels;

namespace Services;

public interface IHealthService
{
    Task<HealthSummaryVm> GetHealthSummaryAsync(string empId);
    Task LogExerciseAsync(ExerciseLog input);
}

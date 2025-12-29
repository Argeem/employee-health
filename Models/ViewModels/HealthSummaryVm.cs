namespace Models.ViewModels;

public class HealthSummaryVm
{
    public string EmpId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double? LatestWeight { get; set; }
    public int ExerciseCount { get; set; }
}

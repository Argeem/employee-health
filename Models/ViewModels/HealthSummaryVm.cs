namespace Models.ViewModels;

public class HealthSummaryVm
{
    public string EmpId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double? LatestWeight { get; set; }
    public int ExerciseCount { get; set; }
    public double? BMI { get; set; }
    public int? MaxHeartRate { get; set; }
    public int QualifiedSessions { get; set; }
    public double SuccessRate { get; set; } // Percentage: (QualifiedSessions / 60) * 100
}

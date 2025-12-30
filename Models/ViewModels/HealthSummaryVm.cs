namespace Models.ViewModels;

public class HealthSummaryVm
{
    public string EmpId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Gender { get; set; }
    public DateTime? Dob { get; set; }
    public decimal? Height { get; set; }
    public double? LatestWeight { get; set; }
    public int ExerciseCount { get; set; }
    public double? BMI { get; set; }
    public int? MaxHeartRate { get; set; }
    public int QualifiedSessions { get; set; }
    public double SuccessRate { get; set; } // Percentage: (QualifiedSessions / 156) * 100
}

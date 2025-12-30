using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities;

public class Employee
{
    public string EmpId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Gender { get; set; }
    public DateTime Dob { get; set; }
    public decimal Height { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class ExerciseLog
{
    public int LogId { get; set; }
    public string EmpId { get; set; } = string.Empty;
    public decimal Weight { get; set; }
    public int? HeartRate { get; set; }
    public string? BloodPressure { get; set; }
    public int DurationMin { get; set; }
    public decimal? DistanceKm { get; set; }
    public string? ImageEvidence { get; set; }
    public DateTime LogDate { get; set; } = DateTime.Now;
    public string? Status { get; set; }
    public string? AdminRemark { get; set; }
}

public class KpiResult
{
    public int KpiId { get; set; }
    public string EmpId { get; set; } = string.Empty;
    public int? PeriodId { get; set; }
    public int? TotalLogs { get; set; }
    public decimal? AttendancePercent { get; set; }
    public decimal? KpiScore { get; set; }
    public string? EvaluationStatus { get; set; }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities;

public enum Gender
{
    Male,
    Female,
    Other
}

public class Employee
{
    [Key]
    public string EmpId { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public Gender? Gender { get; set; }

    public DateTime Dob { get; set; }

    public decimal Height { get; set; }
}

public class ExerciseLog
{
    [Key]
    public int LogId { get; set; }

    public string EmpId { get; set; } = string.Empty;

    public string ActivityName { get; set; } = string.Empty;

    public decimal Weight { get; set; }

    public int? HeartRate { get; set; }

    public string? BloodPressure { get; set; }

    public int DurationMin { get; set; }

    public DateTime LogDate { get; set; } = DateTime.Now;

    public string? ImagePath { get; set; }
}

public class KpiResult
{
    [Key]
    public int KpiId { get; set; }

    public string EmpId { get; set; } = string.Empty;

    public decimal AttendPercent { get; set; }

    public decimal KpiScore { get; set; }

    public string Status { get; set; } = string.Empty;
}

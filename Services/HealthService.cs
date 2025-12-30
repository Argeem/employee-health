using Data;
using Models.Entities;
using Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Services;

public class HealthService : IHealthService
{
    private readonly ApplicationDbContext _db;

    public HealthService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<HealthSummaryVm> GetHealthSummaryAsync(string empId)
    {
        var employee = await _db.FindEmployeeAsync(empId);
        var logs = await _db.ExerciseLogs.Where(l => l.EmpId == empId).OrderByDescending(l => l.LogDate).ToListAsync();

        var latest = logs.FirstOrDefault();
        double? latestWeight = latest != null ? (double?)latest.Weight : null;

        // คำนวณ BMI: Weight (kg) / (Height (m))^2
        double? bmi = null;
        if (employee?.Height > 0 && latestWeight > 0)
        {
            double heightInMeters = (double)employee.Height / 100; // convert cm to meters
            bmi = latestWeight / (heightInMeters * heightInMeters);
        }

        // หา MaxHeartRate จากล็อกทั้งหมด
        int? maxHeartRate = logs.Where(l => l.HeartRate.HasValue).Max(l => l.HeartRate);

        // นับ Qualified Sessions (กิจกรรมที่มีระยะเวลา >= 30 นาที)
        int qualifiedSessions = logs.Count(l => l.DurationMin >= 30);

        // คำนวณ Success Rate: (Qualified Sessions / Target per year) * 100
        // Target = 3 sessions/week × 52 weeks = 156 sessions/year
        double successRate = (qualifiedSessions / 156.0) * 100.0;

        return new HealthSummaryVm
        {
            EmpId = empId,
            Name = employee?.FullName ?? string.Empty,
            LatestWeight = latestWeight,
            ExerciseCount = logs.Count,
            BMI = bmi,
            MaxHeartRate = maxHeartRate,
            QualifiedSessions = qualifiedSessions,
            SuccessRate = successRate
        };
    }

    public async Task<List<ExerciseLog>> GetExerciseLogsAsync(string empId)
    {
        return await _db.ExerciseLogs
            .Where(l => l.EmpId == empId)
            .OrderByDescending(l => l.LogDate)
            .ToListAsync();
    }

    public async Task LogExerciseAsync(ExerciseLog input)
    {
        _db.ExerciseLogs.Add(input);
        await _db.SaveChangesAsync();
    }

    public async Task<List<MonthlyKpiVm>> GetMonthlyKpisAsync(string empId)
    {
        var logs = await _db.ExerciseLogs
            .Where(l => l.EmpId == empId)
            .OrderByDescending(l => l.LogDate)
            .ToListAsync();

        var monthlyGroups = logs.GroupBy(l => new { l.LogDate.Year, l.LogDate.Month })
            .OrderByDescending(g => g.Key.Year)
            .ThenByDescending(g => g.Key.Month)
            .Take(12) // Last 12 months
            .ToList();

        var result = new List<MonthlyKpiVm>();
        string[] thaiMonths = { "", "มกราคม", "กุมภาพันธ์", "มีนาคม", "เมษายน", "พฤษภาคม", "มิถุนายน",
                               "กรกฎาคม", "สิงหาคม", "กันยายน", "ตุลาคม", "พฤศจิกายน", "ธันวาคม" };

        foreach (var group in monthlyGroups)
        {
            var monthLogs = group.ToList();
            int totalSessions = monthLogs.Count;
            int qualifiedSessions = monthLogs.Count(l => l.DurationMin >= 30);
            double successRate = totalSessions > 0 ? (qualifiedSessions / (double)totalSessions) * 100 : 0;
            
            // KPI Score calculation: 
            // Target = 3 sessions per week
            // Average weeks per month = 4.33 (52 weeks / 12 months)
            // Monthly target = 3 × 4.33 ≈ 13 sessions
            double monthlyTarget = 3 * 4.33;
            double kpiScore = Math.Min((qualifiedSessions / monthlyTarget) * 100, 100);
            string status = kpiScore >= 80 ? "ผ่าน" : "ต้องปรับปรุง";

            result.Add(new MonthlyKpiVm
            {
                Year = group.Key.Year,
                Month = group.Key.Month,
                MonthName = thaiMonths[group.Key.Month],
                TotalSessions = totalSessions,
                QualifiedSessions = qualifiedSessions,
                SuccessRate = successRate,
                KpiScore = kpiScore,
                Status = status
            });
        }

        return result;
    }
}

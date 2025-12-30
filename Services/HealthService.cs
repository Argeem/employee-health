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

        return new HealthSummaryVm
        {
            EmpId = empId,
            Name = employee?.FullName ?? string.Empty,
            LatestWeight = latestWeight,
            ExerciseCount = logs.Count
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
}

using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<ExerciseLog> ExerciseLogs { get; set; }
    public DbSet<KpiResult> KpiResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Employee entity
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("employees");
            entity.HasKey(e => e.EmpId);
            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.FullName)
                .HasColumnName("fullname");
            entity.Property(e => e.Gender)
                .HasColumnName("gender");
            entity.Property(e => e.Dob)
                .HasColumnName("dob");
            entity.Property(e => e.Height)
                .HasColumnName("height")
                .HasPrecision(5, 2);
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at");
        });

        // Configure ExerciseLog entity
        modelBuilder.Entity<ExerciseLog>(entity =>
        {
            entity.ToTable("exercise_logs");
            entity.HasKey(e => e.LogId);
            entity.Property(e => e.LogId)
                .HasColumnName("log_id");
            entity.Property(e => e.EmpId)
                .HasColumnName("emp_id")
                .HasMaxLength(20)
                .IsRequired();
            entity.Property(e => e.Weight)
                .HasColumnName("weight")
                .HasPrecision(5, 2);
            entity.Property(e => e.HeartRate)
                .HasColumnName("heart_rate");
            entity.Property(e => e.BloodPressure)
                .HasColumnName("blood_pressure")
                .HasMaxLength(50);
            entity.Property(e => e.DurationMin)
                .HasColumnName("duration_minutes");
            entity.Property(e => e.DistanceKm)
                .HasColumnName("distance_km")
                .HasPrecision(5, 2);
            entity.Property(e => e.ImageEvidence)
                .HasColumnName("image_evidence")
                .HasMaxLength(255);
            entity.Property(e => e.LogDate)
                .HasColumnName("log_date");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(50);
            entity.Property(e => e.AdminRemark)
                .HasColumnName("admin_remark");
        });

        // Configure KpiResult entity
        modelBuilder.Entity<KpiResult>(entity =>
        {
            entity.ToTable("kpi_results");
            entity.HasKey(e => e.KpiId);
            entity.Property(e => e.KpiId)
                .HasColumnName("kpi_id");
            entity.Property(e => e.EmpId)
                .HasColumnName("emp_id")
                .HasMaxLength(20)
                .IsRequired();
            entity.Property(e => e.PeriodId)
                .HasColumnName("period_id");
            entity.Property(e => e.TotalLogs)
                .HasColumnName("total_logs");
            entity.Property(e => e.AttendancePercent)
                .HasColumnName("attendance_p")
                .HasPrecision(5, 2);
            entity.Property(e => e.KpiScore)
                .HasColumnName("kpi_score")
                .HasPrecision(5, 2);
            entity.Property(e => e.EvaluationStatus)
                .HasColumnName("evaluation_sta")
                .HasMaxLength(50);
        });
    }

    public Task<Employee?> FindEmployeeAsync(string empId)
    {
        return Employees.FirstOrDefaultAsync(e => e.EmpId == empId);
    }

    public void AddEmployee(Employee employee)
    {
        Employees.Add(employee);
    }

    public void AddExerciseLog(ExerciseLog log)
    {
        ExerciseLogs.Add(log);
    }
}

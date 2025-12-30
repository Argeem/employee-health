using System.ComponentModel.DataAnnotations;

namespace Models;

public class ExerciseLogInput
{
    public decimal Weight { get; set; }

    public int? HeartRate { get; set; }

    public string? BloodPressure { get; set; }

    public string? ActivityName { get; set; }

    public int DurationMin { get; set; }

    public decimal? DistanceKm { get; set; }

    public IFormFile? ImageEvidence { get; set; }

    public DateTime LogDate { get; set; }
}

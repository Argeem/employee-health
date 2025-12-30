namespace Models.ViewModels;

public class MonthlyKpiVm
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public int TotalSessions { get; set; }
    public int QualifiedSessions { get; set; }
    public double SuccessRate { get; set; }
    public double KpiScore { get; set; } // 0-100
    public string Status { get; set; } = "ผ่าน"; // ผ่าน or ต้องปรับปรุง
}

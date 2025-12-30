using Models.Entities;
using Models.ViewModels;
using Models;
using Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeHealth.Web.Pages;

public class DashboardModel : PageModel
{
    private readonly IHealthService _healthService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<DashboardModel> _logger;

    public DashboardModel(IHealthService healthService, IWebHostEnvironment webHostEnvironment, ILogger<DashboardModel> logger)
    {
        _healthService = healthService;
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
        Input = new ExerciseLogInput();
    }

    [BindProperty(SupportsGet = false)]
    public ExerciseLogInput Input { get; set; }

    public HealthSummaryVm Summary { get; set; } = new();

    public async Task OnGetAsync()
    {
        Summary = await _healthService.GetHealthSummaryAsync("EMP001");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            // Log raw form data
            _logger.LogInformation("=== Form Data Received ===");
            _logger.LogInformation($"Input object: {Input}");
            _logger.LogInformation($"Weight: {Input.Weight} (Type: {Input.Weight.GetType()})");
            _logger.LogInformation($"ActivityName: '{Input.ActivityName}'");
            _logger.LogInformation($"DurationMin: {Input.DurationMin}");
            _logger.LogInformation($"HeartRate: {Input.HeartRate}");
            _logger.LogInformation($"BloodPressure: '{Input.BloodPressure}'");
            _logger.LogInformation($"LogDate: {Input.LogDate}");
            _logger.LogInformation($"ImageEvidence: {Input.ImageEvidence?.FileName ?? "null"}");

            // ตรวจสอบความถูกต้องของข้อมูล
            if (Input.Weight <= 0)
            {
                ModelState.AddModelError("Input.Weight", "น้ำหนักต้องมากกว่า 0");
            }

            if (string.IsNullOrWhiteSpace(Input.ActivityName))
            {
                ModelState.AddModelError("Input.ActivityName", "กรุณาเลือกกิจกรรม");
            }

            if (Input.DurationMin <= 0)
            {
                ModelState.AddModelError("Input.DurationMin", "ระยะเวลาต้องมากกว่า 0 นาที");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Validation failed. ModelState errors: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))}");
                Summary = await _healthService.GetHealthSummaryAsync("EMP001");
                return Page();
            }

            // จัดการอัปโหลดไฟล์
            string? imageFileName = null;
            if (Input.ImageEvidence != null && Input.ImageEvidence.Length > 0)
            {
                try
                {
                    imageFileName = await SaveUploadedFileAsync(Input.ImageEvidence);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Input.ImageEvidence", $"อัปโหลดไฟล์ไม่สำเร็จ: {ex.Message}");
                    Summary = await _healthService.GetHealthSummaryAsync("EMP001");
                    return Page();
                }
            }

            // สร้าง ExerciseLog เพื่อบันทึก
            var exerciseLog = new ExerciseLog
            {
                EmpId = "EMP001",
                Weight = Input.Weight,
                HeartRate = Input.HeartRate,
                BloodPressure = Input.BloodPressure,
                ActivityName = Input.ActivityName,
                DurationMin = Input.DurationMin,
                DistanceKm = Input.DistanceKm,
                ImageEvidence = imageFileName,
                LogDate = Input.LogDate == default ? DateTime.Now : Input.LogDate,
                Status = "Pending"
            };
            
            _logger.LogInformation($"Exercise log prepared - Activity: {exerciseLog.ActivityName}, Duration: {exerciseLog.DurationMin}");
            await _healthService.LogExerciseAsync(exerciseLog);

            TempData["SuccessMessage"] = "บันทึกกิจกรรมสำเร็จ! ข้อมูลของคุณถูกบันทึกลงฐานข้อมูลแล้ว";
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Exception in OnPostAsync: {ex.Message}");
            ModelState.AddModelError("", $"เกิดข้อผิดพลาด: {ex.Message}");
            Summary = await _healthService.GetHealthSummaryAsync("EMP001");
            return Page();
        }
    }

    private async Task<string> SaveUploadedFileAsync(IFormFile file)
    {
        // ตรวจสอบนามสกุลไฟล์
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        if (!allowedExtensions.Contains(fileExtension))
        {
            throw new InvalidOperationException("ไฟล์ต้องเป็นรูปภาพเท่านั้น (jpg, jpeg, png, gif)");
        }

        // ตรวจสอบขนาดไฟล์ (ตัวอย่าง: ไม่เกิน 5MB)
        if (file.Length > 5 * 1024 * 1024)
        {
            throw new InvalidOperationException("ขนาดไฟล์ต้องไม่เกิน 5MB");
        }

        // สร้างชื่อไฟล์ที่ไม่ซ้ำ
        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var uploadsPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "exercise-logs");
        
        // สร้าง directory ถ้าไม่มี
        Directory.CreateDirectory(uploadsPath);
        
        var filePath = Path.Combine(uploadsPath, fileName);
        
        // บันทึกไฟล์
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return fileName;
    }
}

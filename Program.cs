var builder = WebApplication.CreateBuilder(args);

// Configure default encoding to UTF-8 across the entire application
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

// Configure services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Register a simple in-memory ApplicationDbContext
builder.Services.AddSingleton<Data.ApplicationDbContext>();

builder.Services.AddScoped<Services.IHealthService, Services.HealthService>();

var app = builder.Build();

// Seed mock data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Data.ApplicationDbContext>();
    // Seed employee if not exists
    var existing = db.Employees.FirstOrDefault(e => e.EmpId == "EMP-001");
    if (existing == null)
    {
        db.AddEmployee(new Models.Entities.Employee
        {
            EmpId = "EMP-001",
            FullName = "คุณสมชาย ใจแข็งแรง",
            Gender = Models.Entities.Gender.Male,
            Dob = new DateTime(1990, 1, 1),
            Height = 170
        });
        db.SaveChangesAsync().GetAwaiter().GetResult();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Critical: Set UTF-8 charset on ALL HTML responses
app.Use(async (context, next) =>
{
    // Set response content type with UTF-8 charset for HTML responses
    var path = context.Request.Path.Value ?? "";
    
    // Check if this is a Razor page or any HTML request
    bool isHtmlRequest = path.StartsWith("/Dashboard") || 
                         path.StartsWith("/ExerciseLogs") ||
                         path.StartsWith("/KpiResults") ||
                         path.StartsWith("/Profile") ||
                         path == "/" ||
                         path.EndsWith(".html");

    if (isHtmlRequest)
    {
        // Force UTF-8 charset for Razor pages
        context.Response.ContentType = "text/html; charset=utf-8";
    }

    // Ensure charset is added to response even if set elsewhere
    context.Response.OnStarting(() =>
    {
        var contentType = context.Response.ContentType;
        if (contentType != null && contentType.Contains("text/html") && !contentType.Contains("charset"))
        {
            context.Response.ContentType = contentType + "; charset=utf-8";
        }
        return System.Threading.Tasks.Task.CompletedTask;
    });

    await next();
});

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

// Redirect root to Dashboard page since there is no Index page
app.MapGet("/", () => Results.Redirect("/Dashboard"));

// Fallback to Dashboard for unknown routes (helpful for routing issues)
app.MapFallback(() => Results.Redirect("/Dashboard"));

// Log environment for debugging
Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");

app.Run();

# Contributing to EmployeeHealthSystem

This document defines the project structure, coding conventions, and contribution guidelines for the EmployeeHealthSystem solution (Razor Pages targeting .NET 10). Follow these rules exactly when adding code, tests, or configuration so the codebase remains consistent and easy to review.

## Project layout

Root: `EmployeeHealthSystem/`

- `EmployeeHealth.Web/` - Web project (Razor Pages). This is the main entry point for the application and the only web project in the solution.
  - `Controllers/` - API controllers (if required) for health related endpoints (e.g., `HealthController.cs`, `KpiController.cs`, `AccountController.cs`). Controllers should be minimal and delegate business logic to services.
  - `Models/`
    - `Entities/` - EF Core entity classes (`Employee.cs`, `ExerciseLog.cs`, `KpiResult.cs`).
    - `ViewModels/` - DTOs and form models used by Razor Pages (`HealthSummaryVm.cs`, `ExerciseLogInput.cs`).
  - `Services/` - Business logic layer. Keep interfaces and implementations here (e.g., `IHealthService.cs`, `HealthService.cs`). Services must be registered in DI in `Program.cs`.
  - `Data/` - Data access layer. Place EF DbContext and migrations here (`ApplicationDbContext.cs`, `Migrations/`).
  - `Views/` - Razor Pages and shared layouts/partial views. Use Razor Pages structure (Pages folder) or MVC Views only if explicitly needed.
  - `Program.cs` - DI registration and middleware pipeline.
  - `appsettings.json` - Configuration and connection strings.

- `EmployeeHealth.Tests/` - Unit tests for services and other non-UI logic. Use xUnit or NUnit as agreed by the team.

## Naming conventions

- Files: `PascalCase` for class files and types (e.g., `Employee.cs`, `HealthService.cs`).
- Interfaces: prefix with `I` (e.g., `IHealthService`).
- Private fields: `_camelCase` (leading underscore).
- Methods and properties: `PascalCase`.
- Controllers: end with `Controller` (e.g., `HealthController`).

## C# and formatting rules (editorconfig)

The repository must include an `.editorconfig` file. Use the following rules as a minimum:

- Charset: `utf-8`
- Indentation: 4 spaces
- Insert final newline
- Trim trailing whitespace
- `csharp_prefer_braces = true`
- `dotnet_style_qualification_for_field = false`
- `csharp_style_var_for_built_in_types = true`

The maintainer will ensure a complete `.editorconfig` is present. Respect that file for any formatting/analysis rules.

## Code style

- Make methods small and single-purpose.
- Business logic belongs in `Services/` (Do not put heavy logic in controllers or pages).
- Use dependency injection; avoid static state.
- Use EF Core for persistence and follow proper async patterns (e.g., `await context.SaveChangesAsync()`).

## Tests

- Write unit tests for all public service methods and critical business rules.
- Keep tests in `EmployeeHealth.Tests/` with a parallel namespace to the code under test.

## Pull requests

- Target the `main` branch with small, focused pull requests.
- Provide a clear description and reference any related issues.
- Ensure all unit tests pass locally and CI.

## Tooling and Visual Studio

- This project targets .NET 10 - set `TargetFramework` to `net10.0`.
- Open the solution in Visual Studio 2026. The workspace uses default solution-level settings; adjust only via the `.editorconfig` and `Directory.Build.props` if necessary.

## Adding new files and folders

- Add new types to the appropriate folder (e.g., Entities to `Models/Entities`).
- Register service implementations in `Program.cs` using `AddScoped`/`AddSingleton` as appropriate.
- Add EF Core migrations into `Data/Migrations`.

If anything in this file conflicts with a team-wide policy or `Directory.Build.props`/`.editorconfig`, the team policy takes precedence.

using Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data;

public class ApplicationDbContext
{
    private readonly List<Employee> _employees = new();
    private readonly List<ExerciseLog> _exerciseLogs = new();

    public IEnumerable<Employee> Employees => _employees;
    public IEnumerable<ExerciseLog> ExerciseLogs => _exerciseLogs;

    public Task<Employee?> FindEmployeeAsync(string empId)
    {
        var emp = _employees.FirstOrDefault(e => e.EmpId == empId);
        return Task.FromResult(emp);
    }

    public void AddEmployee(Employee employee)
    {
        _employees.Add(employee);
    }

    public void AddExerciseLog(ExerciseLog log)
    {
        _exerciseLogs.Add(log);
    }

    public Task SaveChangesAsync()
    {
        // In-memory; nothing to persist.
        return Task.CompletedTask;
    }
}

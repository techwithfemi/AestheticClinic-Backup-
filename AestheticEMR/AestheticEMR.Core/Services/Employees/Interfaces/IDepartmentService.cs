using AestheticEMR.Core.Models.Employees;

namespace AestheticEMR.Core.Services.Employees.Interfaces;

public interface IDepartmentService
{
    /// <summary>
    /// Previews the next 2-digit department id (e.g. "01") without committing it.
    /// Mirrors the legacy VB.NET <c>genIDNo()</c> behaviour: <c>max(deptID) + 1</c>,
    /// right-padded to 2 characters, capped at "99".
    /// </summary>
    Task<string> GenerateDepartmentIdAsync();

    Task<IEnumerable<EmpDepartments>> GetAllAsync();

    Task<EmpDepartments?> GetByIdAsync(string deptId);

    /// <summary>
    /// Creates a new department, atomically generating and committing the next id.
    /// </summary>
    Task<EmpDepartments> CreateAsync(EmpDepartments department);

    Task<EmpDepartments> UpdateAsync(EmpDepartments department);

    /// <summary>
    /// Deletes a department. Returns <c>true</c> when a row was removed, <c>false</c> when
    /// no record with that id existed. Throws <see cref="InvalidOperationException"/> when
    /// the department is still referenced by one or more employees.
    /// </summary>
    Task<bool> DeleteAsync(string deptId);

    /// <summary>
    /// True if any employee row currently references this department id.
    /// Used by the controller to surface a 409 Conflict on delete.
    /// </summary>
    Task<bool> IsInUseAsync(string deptId);

    /// <summary>
    /// Returns a map of department id → employee count for every department that has
    /// at least one employee referencing it. Used by the list endpoint to populate
    /// <see cref="ViewModels.Employees.DepartmentVM.InUseCount"/>.
    /// </summary>
    Task<IReadOnlyDictionary<string, int>> GetInUseCountsAsync();
}

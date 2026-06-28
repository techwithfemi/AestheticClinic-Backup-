using AestheticEMR.Core.Models.Employees;

namespace AestheticEMR.Core.Services.Employees.Interfaces;

public interface IDesignationService
{
    /// <summary>
    /// Previews the next 2-digit designation id (e.g. "01") without committing it.
    /// Mirrors the legacy VB.NET <c>genIDNo()</c> behaviour: <c>max(desID) + 1</c>,
    /// right-padded to 2 characters, capped at "99".
    /// </summary>
    Task<string> GenerateDesignationIdAsync();

    Task<IEnumerable<Designation>> GetAllAsync();

    Task<Designation?> GetByIdAsync(string desId);

    /// <summary>
    /// Creates a new designation, atomically generating and committing the next id.
    /// </summary>
    Task<Designation> CreateAsync(Designation designation);

    Task<Designation> UpdateAsync(Designation designation);

    /// <summary>
    /// Deletes a designation. Returns <c>true</c> when a row was removed, <c>false</c> when
    /// no record with that id existed. Throws <see cref="InvalidOperationException"/> when
    /// the designation is still referenced by one or more employees.
    /// </summary>
    Task<bool> DeleteAsync(string desId);

    /// <summary>
    /// True if any employee row currently references this designation id.
    /// Used by the controller to surface a 409 Conflict on delete.
    /// </summary>
    Task<bool> IsInUseAsync(string desId);

    /// <summary>
    /// Returns a map of designation id → employee count for every designation that has
    /// at least one employee referencing it. Used by the list endpoint to populate
    /// <see cref="ViewModels.Employees.DesignationVM.InUseCount"/>.
    /// </summary>
    Task<IReadOnlyDictionary<string, int>> GetInUseCountsAsync();
}
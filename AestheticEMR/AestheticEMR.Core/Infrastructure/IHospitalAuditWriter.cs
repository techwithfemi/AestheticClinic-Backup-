using System.Collections.Generic;
using System.Threading.Tasks;

namespace AestheticEMR.Core.Infrastructure;

/// <summary>
/// Lightweight audit writer for services that use raw Dapper/SqlConnection
/// directly and therefore bypass AuditedSqlDataAccess.
/// Call WriteAsync after every successful write (create/update/delete).
/// </summary>
public interface IHospitalAuditWriter
{
    /// <summary>
    /// Writes one row to Auditrail.
    /// </summary>
    /// <param name="tranCode">consultID / billNo / pNo / desID / DeptId — primary transaction key.</param>
    /// <param name="eventType">Create | Update | Delete</param>
    /// <param name="src">Entity or page name (e.g. "EmpDepartments").</param>
    /// <param name="auditCat">Module name (e.g. "employees").</param>
    /// <param name="payload">Dictionary of input-label → value pairs that become the JSON UserAction (new values).</param>
    /// <param name="originalPayload">Dictionary of original values (before the update). Optional; used for Update operations.</param>
    Task WriteAsync(string tranCode, string eventType, string src, string auditCat,
        IReadOnlyDictionary<string, object?> payload,
        IReadOnlyDictionary<string, object?>? originalPayload = null);
}

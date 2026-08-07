using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Services.Account;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess.DbAccess;

/// <summary>
/// Decorator over <see cref="SqlDataAccess"/> that automatically writes a row to the
/// hospital <c>Auditrail</c> table after every successful Dapper write operation,
/// following the same column rules as HospitalAuditTrailInterceptor.
/// </summary>
public sealed class AuditedSqlDataAccess(
    SqlDataAccess inner,
    IUserIdAccessor userIdAccessor,
    AuditRequestContext auditRequestContext,
    ILogger<AuditedSqlDataAccess> logger) : ISqlDataAccess
{
    private const string AuditConnectionId = "DefaultConnection";

    private const string InsertAuditSql =
        "INSERT INTO Auditrail (TranCode, UserName, UserAction, OriginalAction, ActionDate, ActionTime, Remarks, Src, AuditCat) " +
        "VALUES (@TranCode, @UserName, @UserAction, @OriginalAction, @ActionDate, @ActionTime, @Remarks, @Src, @AuditCat);";

    // ── Read-only pass-throughs ───────────────────────────────────────────────

    public Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionId)
        => inner.LoadData<T, U>(storedProcedure, parameters, connectionId);

    public Task<IEnumerable<T>> LoadDataText<T, U>(string query, U parameters, string connectionId)
        => inner.LoadDataText<T, U>(query, parameters, connectionId);

    // ── Write operations — execute then audit ────────────────────────────────

    public async Task SaveData<T>(string storedProcedure, T parameters, string connectionId)
    {
        await inner.SaveData(storedProcedure, parameters, connectionId);
        await WriteAuditAsync(storedProcedure, connectionId, parameters, isStoredProcedure: true);
    }

    public async Task SaveDataText<T>(string query, T parameters, string connectionId)
    {
        await inner.SaveDataText(query, parameters, connectionId);
        await WriteAuditAsync(query, connectionId, parameters, isStoredProcedure: false);
    }

    // ── Audit helpers ─────────────────────────────────────────────────────────

    private async Task WriteAuditAsync<T>(string operation, string connectionId, T parameters, bool isStoredProcedure)
    {
        // Never audit the audit INSERT itself — would cause infinite loop.
        if (operation.Contains("Auditrail", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        try
        {
            var utcToday = DateTime.UtcNow.Date;
            var localToday = DateTime.Now.Date;
            var localNow = DateTime.Now;
            var userName = Truncate(userIdAccessor.GetCurrentUserId() ?? "SYSTEM", 50);
            var tranCode = Truncate(ExtractTranCode(parameters), 50);
            var eventType = ResolveEventType(operation, isStoredProcedure);
            var src = Truncate(BuildSourceMetadata(operation, connectionId, isStoredProcedure), 1000);

            // UserAction: JSON payload using parameter values
            var payload = BuildPayloadJson(parameters);
            var userAction = Truncate(payload, 5000);

            // Remarks: type of CRUD operation
            var priKey = ExtractPriKey(parameters);
            var remarks = eventType switch
            {
                "Create" => "created record",
                "Update" => $"updated record with priKey: {priKey}",
                "Delete" => $"deleted record with priKey: {priKey}",
                _ => $"{eventType} record"
            };

            // AuditCat: module derived from SP name convention (e.g. InsertEmpDesig → employees)
            var auditCat = Truncate(ResolveAuditCat(operation, connectionId), 1000);

            // Note: AuditedSqlDataAccess does not capture OriginalAction (before values) 
            // because it audits AFTER the write. Services with explicit before/after data
            // should use IHospitalAuditWriter.WriteAsync() with originalPayload parameter instead.
            await inner.SaveDataText(InsertAuditSql, new
            {
                TranCode = tranCode,
                UserName = userName,
                UserAction = userAction,
                OriginalAction = (string?)null,
                ActionDate = localToday,
                ActionTime = localNow,
                Remarks = Truncate(remarks, 8000),
                Src = src,
                AuditCat = auditCat
            }, AuditConnectionId);
        }
        catch (Exception ex)
        {
            // Audit failure must never fail the actual business write — log and swallow.
            logger.LogWarning(ex, "Dapper audit write failed for operation '{Operation}' on connection '{ConnectionId}'",
                operation, connectionId);
        }
    }

    /// <summary>
    /// Resolves a short display name for the operation (SP name or first SQL keyword).
    /// </summary>
    private static string ResolveOperationName(string operation, bool isStoredProcedure)
    {
        if (isStoredProcedure)
            return operation.Trim();

        var trimmed = operation.AsSpan().TrimStart();
        var spaceIndex = trimmed.IndexOf(' ');
        return spaceIndex > 0 ? trimmed[..spaceIndex].ToString() : trimmed.ToString();
    }

    /// <summary>
    /// Infers CRUD event type from operation name.
    /// </summary>
    private static string ResolveEventType(string operation, bool isStoredProcedure)
    {
        var lower = (isStoredProcedure ? operation : ResolveOperationName(operation, false)).ToLowerInvariant();

        if (lower.StartsWith("insert", StringComparison.Ordinal) || lower.StartsWith("add", StringComparison.Ordinal)
            || lower.StartsWith("new", StringComparison.Ordinal) || lower.StartsWith("create", StringComparison.Ordinal))
            return "Create";

        if (lower.StartsWith("update", StringComparison.Ordinal) || lower.StartsWith("edit", StringComparison.Ordinal)
            || lower.StartsWith("save", StringComparison.Ordinal))
            return "Update";

        if (lower.StartsWith("delete", StringComparison.Ordinal) || lower.StartsWith("remove", StringComparison.Ordinal))
            return "Delete";

        return "Write";
    }

    /// <summary>
    /// Derives AuditCat from the business module of the write operation.
    /// </summary>
    private static string ResolveAuditCat(string operation, string connectionId)
    {
        var lower = operation.ToLowerInvariant();

        if (lower.Contains("bill") || lower.Contains("payment") || lower.Contains("receipt") || lower.Contains("invoice")) return "billing";
        if (lower.Contains("dental") || lower.Contains("tooth") || lower.Contains("odont") || lower.Contains("imaging")) return "dental";
        if (lower.Contains("consult") || lower.Contains("patient") || lower.Contains("record") || lower.Contains("retainership") || lower.Contains("referal") || lower.Contains("appointment")) return "frontDesk";
        if (lower.Contains("desig") || lower.Contains("dept") || lower.Contains("department") || lower.Contains("emp") || lower.Contains("employee") || lower.Contains("roster") || lower.Contains("shift")) return "employees";
        if (lower.Contains("aesthetic")) return "aesthetics";
        if (lower.Contains("tran") || lower.Contains("journal") || lower.Contains("income") || lower.Contains("expense") || lower.Contains("account")) return "accounting";

        var conn = connectionId.ToLowerInvariant();
        if (conn.Contains("smart") || conn.Contains("hr")) return "employees";
        if (conn.Contains("account") || conn.Contains("acct")) return "accounting";

        return "general";
    }

    /// <summary>
    /// Extracts a transaction code from known parameter property names.
    /// </summary>
    private static string ExtractTranCode(object? parameters)
    {
        if (parameters is null) return "GENERAL";

        var candidates = new[]
        {
            "TranCode", "consultID", "ConsultId", "BillNo", "billNO",
            "PNo", "Pno", "LabNo", "TranNo", "TranID", "desID", "EmpID"
        };

        var type = parameters.GetType();
        foreach (var name in candidates)
        {
            var prop = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop is null) continue;
            var value = prop.GetValue(parameters)?.ToString()?.Trim();
            if (!string.IsNullOrWhiteSpace(value)) return value;
        }

        return "GENERAL";
    }

    /// <summary>
    /// Extracts primary key value for Remarks field.
    /// </summary>
    private static string ExtractPriKey(object? parameters)
    {
        if (parameters is null) return "unknown";

        var candidates = new[]
        {
            "Id", "SNo", "SNO", "desID", "EmpID", "TranNo", "TranID",
            "consultID", "BillNo", "billNO", "PNo", "Pno", "LabNo"
        };

        var type = parameters.GetType();
        foreach (var name in candidates)
        {
            var prop = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop is null) continue;
            var value = prop.GetValue(parameters)?.ToString()?.Trim();
            if (!string.IsNullOrWhiteSpace(value)) return value;
        }

        return "unknown";
    }

    /// <summary>
    /// Serializes parameters as a JSON object using property names as keys (matching ViewModel names).
    /// </summary>
    private static string BuildPayloadJson(object? parameters)
    {
        if (parameters is null) return "{}";

        var type = parameters.GetType();
        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        if (props.Length == 0) return "{}";

        var dict = new Dictionary<string, object?>();
        foreach (var prop in props)
        {
            if (prop.PropertyType == typeof(byte[]) || prop.PropertyType == typeof(byte?[])) continue;
            var value = prop.GetValue(parameters);
            if (value is null) continue;
            dict[prop.Name] = value;
        }

        return dict.Count == 0 ? "{}" : JsonSerializer.Serialize(dict);
    }

    private static string Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value)) return string.Empty;
        var trimmed = value.Trim();
        return trimmed.Length <= maxLength ? trimmed : trimmed[..maxLength];
    }

    private string BuildSourceMetadata(string operation, string connectionId, bool isStoredProcedure)
    {
        var metadata = new Dictionary<string, object?>
        {
            ["operation"] = ResolveOperationName(operation, isStoredProcedure),
            ["connectionId"] = connectionId,
            ["requestPath"] = auditRequestContext.GetRequestPath(),
            ["deviceName"] = auditRequestContext.GetDeviceName(),
            ["ipAddress"] = auditRequestContext.GetIpAddress(),
            ["userAgent"] = auditRequestContext.GetUserAgent(),
            ["city"] = auditRequestContext.GetCity(),
            ["country"] = auditRequestContext.GetCountry(),
            ["coordinates"] = auditRequestContext.GetCoordinates(),
            ["module"] = ResolveAuditCat(operation, connectionId)
        };

        var filtered = metadata
            .Where(x => x.Value is not null && !string.IsNullOrWhiteSpace(x.Value.ToString()))
            .ToDictionary(x => x.Key, x => x.Value);

        return filtered.Count == 0
            ? ResolveOperationName(operation, isStoredProcedure)
            : JsonSerializer.Serialize(filtered);
    }
}

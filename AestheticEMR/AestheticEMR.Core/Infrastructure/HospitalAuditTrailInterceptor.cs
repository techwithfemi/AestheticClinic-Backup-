using AestheticEMR.Core.Models.Aesthetic;
using AestheticEMR.Core.Services.Account;
using DataAccess.DbAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AestheticEMR.Core.Infrastructure;

public sealed class HospitalAuditTrailInterceptor(
    ISqlDataAccess sqlDataAccess,
    IUserIdAccessor userIdAccessor,
    AuditRequestContext auditRequestContext) : SaveChangesInterceptor
{
    private const string DefaultConnectionId = "DefaultConnection";
    private static readonly ConcurrentDictionary<Guid, List<HospitalAuditEntry>> PendingEntries = new();

    // Namespaces and table-name prefixes that must never be audited
    private static readonly string[] ExcludedNamespacePrefixes =
    [
        "OpenIddict",
        "Microsoft.AspNetCore.Identity",
    ];

    private static readonly string[] ExcludedTablePrefixes =
    [
        "AspNet", "vw", "qry", "OpenIddict",
    ];

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        CaptureEntries(eventData.Context);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        CaptureEntries(eventData.Context);
        return ValueTask.FromResult(result);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        PersistEntriesAsync(eventData.Context, CancellationToken.None).GetAwaiter().GetResult();
        return result;
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        await PersistEntriesAsync(eventData.Context, cancellationToken);
        return result;
    }

    public override void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        ClearPending(eventData.Context);
        base.SaveChangesFailed(eventData);
    }

    public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        ClearPending(eventData.Context);
        return base.SaveChangesFailedAsync(eventData, cancellationToken);
    }

    private sealed record HospitalAuditEntry(
        string TranCode,
        string UserName,
        string UserAction,
        string? OriginalAction,
        DateTime ActionDate,
        DateTime ActionTime,
        string? Remarks,
        string? Src,
        string? AuditCat);

    private void CaptureEntries(DbContext? context)
    {
        if (context is null || !IsAuditTrailTableAvailable(context))
        {
            return;
        }

        var utcToday = DateTime.UtcNow.Date;
        var localToday = DateTime.Now.Date;
        var localNow = DateTime.Now;
        var currentUser = SafeTrim(userIdAccessor.GetCurrentUserId(), 50) ?? "SYSTEM";

        var entries = context.ChangeTracker.Entries()
            .Where(e => e.Entity is not AuditLog
                        && (e.State == EntityState.Added
                            || e.State == EntityState.Modified
                            || e.State == EntityState.Deleted)
                        && ShouldAuditEntry(e))
            .Select(entry =>
            {
                var eventType = entry.State == EntityState.Added
                    ? "Create"
                    : entry.State == EntityState.Modified
                        ? "Update"
                        : "Delete";

                var entityType = entry.Metadata.ClrType.Name;
                var priKey = ResolveEntityId(entry)?.ToString() ?? "unknown";

                // UserAction: JSON payload with current values (ViewModel property names as keys)
                var userAction = BuildUserActionJson(entry, eventType);
                if (string.IsNullOrWhiteSpace(userAction) && eventType != "Delete")
                {
                    return null;
                }

                // OriginalAction: JSON payload with original values (for update/delete) or null (for create)
                var originalAction = BuildOriginalActionJson(entry, eventType);

                // Remarks: type of CRUD operation
                var remarks = eventType switch
                {
                    "Create" => $"created record",
                    "Update" => $"updated record with priKey: {priKey}",
                    "Delete" => $"deleted record with priKey: {priKey}",
                    _ => $"{eventType} record with priKey: {priKey}"
                };

                // AuditCat: module name derived from entity namespace
                var auditCat = ResolveAuditCat(entry);

                // Src: request/device/location metadata showing where the CRUD originates
                var src = BuildSourceMetadata(entry, eventType);

                return new HospitalAuditEntry(
                    TranCode: SafeTrim(ResolveTranCode(entry), 50) ?? "GENERAL",
                    UserName: currentUser,
                    UserAction: SafeTrim(userAction, 5000) ?? userAction,
                    OriginalAction: SafeTrim(originalAction, 5000),
                    ActionDate: localToday,
                    ActionTime: localNow,
                    Remarks: SafeTrim(remarks, 8000),
                    Src: SafeTrim(src, 1000),
                    AuditCat: SafeTrim(auditCat, 1000));
            })
            .Where(x => x is not null)
            .Cast<HospitalAuditEntry>()
            .ToList();

        if (entries.Count > 0)
        {
            PendingEntries[context.ContextId.InstanceId] = entries;
        }
    }

    private async Task PersistEntriesAsync(DbContext? context, CancellationToken cancellationToken)
    {
        if (context is null)
        {
            return;
        }

        if (!PendingEntries.TryRemove(context.ContextId.InstanceId, out var entries) || entries.Count == 0)
        {
            return;
        }

        const string insertSql = @"
INSERT INTO Auditrail (TranCode, UserName, UserAction, OriginalAction, ActionDate, ActionTime, Remarks, Src, AuditCat)
VALUES (@TranCode, @UserName, @UserAction, @OriginalAction, @ActionDate, @ActionTime, @Remarks, @Src, @AuditCat);";

        foreach (var entry in entries)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await sqlDataAccess.SaveDataText(insertSql, new
            {
                entry.TranCode,
                entry.UserName,
                entry.UserAction,
                entry.OriginalAction,
                entry.ActionDate,
                entry.ActionTime,
                entry.Remarks,
                entry.Src,
                entry.AuditCat
            }, DefaultConnectionId);
        }
    }

    private static bool IsAuditTrailTableAvailable(DbContext context)
    {
        try
        {
            var connection = context.Database.GetDbConnection();
            var shouldClose = connection.State != ConnectionState.Open;

            if (shouldClose)
            {
                connection.Open();
            }

            try
            {
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Auditrail'";
                var result = command.ExecuteScalar();
                return result != null && result != DBNull.Value;
            }
            finally
            {
                if (shouldClose)
                {
                    connection.Close();
                }
            }
        }
        catch
        {
            return false;
        }
    }

    private static void ClearPending(DbContext? context)
    {
        if (context is not null)
        {
            PendingEntries.TryRemove(context.ContextId.InstanceId, out _);
        }
    }

    private static bool ShouldAuditEntry(EntityEntry entry)
    {
        var clrType = entry.Metadata.ClrType;
        var entityNamespace = clrType.Namespace ?? string.Empty;

        foreach (var prefix in ExcludedNamespacePrefixes)
        {
            if (entityNamespace.StartsWith(prefix, StringComparison.Ordinal))
                return false;
        }

        var tableName = entry.Metadata.GetTableName() ?? string.Empty;
        foreach (var prefix in ExcludedTablePrefixes)
        {
            if (tableName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return false;
        }

        return true;
    }

    private static string ResolveTranCode(EntityEntry entry)
    {
        var candidatePropertyNames = new[]
        {
            "ConsultId", "consultID", "BillNo", "billNO", "PNo", "Pno", "LabNo", "TranCode"
        };

        foreach (var propertyName in candidatePropertyNames)
        {
            var property = entry.Properties.FirstOrDefault(p =>
                string.Equals(p.Metadata.Name, propertyName, StringComparison.OrdinalIgnoreCase));

            if (property is null)
            {
                continue;
            }

            var rawValue = entry.State == EntityState.Deleted ? property.OriginalValue : property.CurrentValue;
            var value = rawValue?.ToString()?.Trim();

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }

        return "GENERAL";
    }

    private static int? ResolveEntityId(EntityEntry entry)
    {
        var primaryKey = entry.Metadata.FindPrimaryKey();
        var keyProperty = primaryKey?.Properties.FirstOrDefault();
        if (keyProperty is null) return null;

        var keyEntry = entry.Property(keyProperty.Name);
        var rawValue = entry.State == EntityState.Deleted ? keyEntry.OriginalValue : keyEntry.CurrentValue;

        if (rawValue is int intValue) return intValue;
        if (rawValue is long longValue && longValue <= int.MaxValue) return (int)longValue;
        if (rawValue is string strValue && int.TryParse(strValue, out var parsed)) return parsed;

        return null;
    }

    /// <summary>
    /// Builds UserAction JSON: current values for Create, only changed fields for Update, minimal key info for Delete.
    /// Uses property names (ViewModel names) as keys, not human-readable labels.
    /// </summary>
    private static string BuildUserActionJson(EntityEntry entry, string eventType)
    {
        var properties = entry.Properties.Where(p =>
            !p.Metadata.IsPrimaryKey()
            && p.Metadata.Name != "CreatedBy"
            && p.Metadata.Name != "CreatedDate"
            && p.Metadata.Name != "UpdatedBy"
            && p.Metadata.Name != "UpdatedDate"
            && p.Metadata.ClrType != typeof(byte[]));

        var dict = new Dictionary<string, object?>();

        switch (eventType)
        {
            case "Create":
                foreach (var p in properties.Where(p => p.CurrentValue != null))
                    dict[p.Metadata.Name] = p.CurrentValue;
                break;

            case "Update":
                foreach (var p in properties.Where(p => p.IsModified && !Equals(p.OriginalValue, p.CurrentValue)))
                    dict[p.Metadata.Name] = p.CurrentValue;
                break;

            case "Delete":
                var primaryKey = entry.Metadata.FindPrimaryKey();
                if (primaryKey is not null)
                {
                    foreach (var keyProperty in primaryKey.Properties)
                    {
                        var keyEntry = entry.Property(keyProperty.Name);
                        var keyValue = keyEntry.OriginalValue ?? keyEntry.CurrentValue;
                        if (keyValue is not null)
                        {
                            dict[keyProperty.Name] = keyValue;
                        }
                    }
                }
                break;
        }

        if (dict.Count == 0) return string.Empty;

        return JsonSerializer.Serialize(dict);
    }

    /// <summary>
    /// Builds OriginalAction JSON: old values for Update/Delete, null for Create.
    /// Uses property names (ViewModel names) as keys.
    /// </summary>
    private static string? BuildOriginalActionJson(EntityEntry entry, string eventType)
    {
        if (eventType == "Create")
            return null;

        var properties = entry.Properties.Where(p =>
            !p.Metadata.IsPrimaryKey()
            && p.Metadata.Name != "CreatedBy"
            && p.Metadata.Name != "CreatedDate"
            && p.Metadata.Name != "UpdatedBy"
            && p.Metadata.Name != "UpdatedDate"
            && p.Metadata.ClrType != typeof(byte[]));

        var dict = new Dictionary<string, object?>();

        switch (eventType)
        {
            case "Update":
                // For update, include only changed fields with their old values
                foreach (var p in properties.Where(p => p.IsModified && !Equals(p.OriginalValue, p.CurrentValue)))
                    dict[p.Metadata.Name] = p.OriginalValue;
                break;

            case "Delete":
                // For delete, include all original values (full deleted record for recovery)
                foreach (var p in properties.Where(p => p.OriginalValue != null))
                    dict[p.Metadata.Name] = p.OriginalValue;
                break;
        }

        if (dict.Count == 0) return null;

        return JsonSerializer.Serialize(dict);
    }

    /// <summary>
    /// Removed: ToLabel() method is no longer used. Property names are serialized as-is.
    /// This ensures clean JSON keys like "desName" instead of "des name".
    /// </summary>

    /// <summary>
    /// Derives module/AuditCat from the business area where the CRUD happened.
    /// </summary>
    private static string ResolveAuditCat(EntityEntry entry)
    {
        var entityName = entry.Metadata.ClrType.Name;
        var tableName = entry.Metadata.GetTableName() ?? entityName;
        var clinic = GetPropertyValue(entry, "Clinic")?.ToLowerInvariant();
        var source = $"{entityName} {tableName}".ToLowerInvariant();

        if (!string.IsNullOrWhiteSpace(clinic))
        {
            if (clinic.Contains("dental")) return "dental";
            if (clinic.Contains("bill")) return "billing";
            if (clinic.Contains("front") || clinic.Contains("record") || clinic.Contains("consult")) return "frontDesk";
        }

        if (source.Contains("billing") || source.Contains("billaccum") || source.Contains("payment") || source.Contains("receipt"))
            return "billing";

        if (source.Contains("dental") || source.Contains("tooth") || source.Contains("odont") || source.Contains("imaging"))
            return "dental";

        if (source.Contains("patient") || source.Contains("consult") || source.Contains("record") || source.Contains("retainership") || source.Contains("referal") || source.Contains("appointment"))
            return "frontDesk";

        if (source.Contains("employee") || source.Contains("designation") || source.Contains("department") || source.Contains("roster") || source.Contains("shift"))
            return "employees";

        if (source.Contains("aesthetic"))
            return "aesthetics";

        if (source.Contains("journal") || source.Contains("tranxaction") || source.Contains("expense") || source.Contains("income") || source.Contains("account"))
            return "accounting";

        var ns = entry.Metadata.ClrType.Namespace ?? string.Empty;
        const string modelsMarker = ".Models.";
        var idx = ns.IndexOf(modelsMarker, StringComparison.OrdinalIgnoreCase);
        if (idx >= 0)
        {
            var segment = ns[(idx + modelsMarker.Length)..];
            var dotIdx = segment.IndexOf('.');
            var namespaceSegment = (dotIdx > 0 ? segment[..dotIdx] : segment).ToLowerInvariant();

            return namespaceSegment switch
            {
                "legacy" => "frontDesk",
                "dental" => "dental",
                "aesthetic" => "aesthetics",
                "employees" => "employees",
                "accounting" => "accounting",
                _ => namespaceSegment
            };
        }

        return "general";
    }

    private static string? GetPropertyValue(EntityEntry entry, string propertyName)
    {
        var property = entry.Properties.FirstOrDefault(p =>
            string.Equals(p.Metadata.Name, propertyName, StringComparison.OrdinalIgnoreCase));

        if (property is null)
        {
            return null;
        }

        var rawValue = entry.State == EntityState.Deleted ? property.OriginalValue : property.CurrentValue;
        return rawValue?.ToString()?.Trim();
    }

    private static string? SafeTrim(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var trimmed = value.Trim();
        return trimmed.Length <= maxLength ? trimmed : trimmed[..maxLength];
    }

    private string BuildSourceMetadata(EntityEntry entry, string eventType)
    {
        var metadata = new Dictionary<string, object?>
        {
            ["eventType"] = eventType,
            ["entityName"] = entry.Metadata.ClrType.Name,
            ["tableName"] = entry.Metadata.GetTableName(),
            ["requestPath"] = auditRequestContext.GetRequestPath(),
            ["deviceName"] = auditRequestContext.GetDeviceName(),
            ["ipAddress"] = auditRequestContext.GetIpAddress(),
            ["userAgent"] = auditRequestContext.GetUserAgent(),
            ["city"] = auditRequestContext.GetCity(),
            ["country"] = auditRequestContext.GetCountry(),
            ["coordinates"] = auditRequestContext.GetCoordinates()
        };

        var clinic = GetPropertyValue(entry, "Clinic");
        if (!string.IsNullOrWhiteSpace(clinic))
        {
            metadata["clinic"] = clinic;
        }

        var routeModule = ResolveAuditCat(entry);
        if (!string.IsNullOrWhiteSpace(routeModule))
        {
            metadata["module"] = routeModule;
        }

        var filtered = metadata
            .Where(x => x.Value is not null && !string.IsNullOrWhiteSpace(x.Value.ToString()))
            .ToDictionary(x => x.Key, x => x.Value);

        return filtered.Count == 0
            ? entry.Metadata.ClrType.Name
            : JsonSerializer.Serialize(filtered);
    }
}

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

public sealed class HospitalAuditTrailInterceptor(ISqlDataAccess sqlDataAccess, IUserIdAccessor userIdAccessor) : SaveChangesInterceptor
{
    private const string DefaultConnectionId = "DefaultConnection";
    private static readonly ConcurrentDictionary<Guid, List<HospitalAuditEntry>> PendingEntries = new();

    // Namespaces and table-name prefixes that must never be audited
    private static readonly string[] ExcludedNamespacePrefixes =
    [
        "AestheticEMR.Core.Models.Legacy",
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

                // UserAction: JSON payload with input labels as keys (rule: use input labels, not model names)
                var payload = BuildPayloadJson(entry, eventType);
                if (string.IsNullOrWhiteSpace(payload))
                {
                    return null;
                }

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

                // Src: entity/page name where the payload originates
                var src = entityType;

                return new HospitalAuditEntry(
                    TranCode: SafeTrim(ResolveTranCode(entry), 50) ?? "GENERAL",
                    UserName: currentUser,
                    UserAction: SafeTrim(payload, 5000) ?? payload,
                    ActionDate: localToday,
                    ActionTime: localNow,
                    Remarks: SafeTrim(remarks, 8000),
                    Src: SafeTrim(src, 150),
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
INSERT INTO Auditrail (TranCode, UserName, UserAction, ActionDate, ActionTime, Remarks, Src, AuditCat)
VALUES (@TranCode, @UserName, @UserAction, @ActionDate, @ActionTime, @Remarks, @Src, @AuditCat);";

        foreach (var entry in entries)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await sqlDataAccess.SaveDataText(insertSql, new
            {
                entry.TranCode,
                entry.UserName,
                entry.UserAction,
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
    /// Builds a JSON object where keys are human-readable input labels (not model property names).
    /// For updates, only changed fields are included showing old → new.
    /// </summary>
    private static string BuildPayloadJson(EntityEntry entry, string eventType)
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
                    dict[ToLabel(p.Metadata.Name)] = p.CurrentValue;
                break;

            case "Delete":
                foreach (var p in properties.Where(p => p.OriginalValue != null))
                    dict[ToLabel(p.Metadata.Name)] = p.OriginalValue;
                break;

            case "Update":
                foreach (var p in properties.Where(p => p.IsModified && !Equals(p.OriginalValue, p.CurrentValue)))
                    dict[ToLabel(p.Metadata.Name)] = $"{p.OriginalValue} → {p.CurrentValue}";
                break;
        }

        if (dict.Count == 0) return string.Empty;

        return JsonSerializer.Serialize(dict);
    }

    /// <summary>
    /// Converts a PascalCase/camelCase property name to a readable label
    /// e.g. "desName" → "des name", "PatientNo" → "patient no"
    /// </summary>
    private static string ToLabel(string propertyName)
    {
        if (string.IsNullOrEmpty(propertyName)) return propertyName;

        var sb = new System.Text.StringBuilder();
        for (var i = 0; i < propertyName.Length; i++)
        {
            var c = propertyName[i];
            if (i > 0 && char.IsUpper(c))
                sb.Append(' ');
            sb.Append(char.ToLowerInvariant(c));
        }
        return sb.ToString();
    }

    /// <summary>
    /// Derives module/AuditCat from entity namespace segment.
    /// e.g. AestheticEMR.Core.Models.Employees → employees
    ///      AestheticEMR.Core.Models.Aesthetic  → aesthetics
    /// </summary>
    private static string ResolveAuditCat(EntityEntry entry)
    {
        var ns = entry.Metadata.ClrType.Namespace ?? string.Empty;

        // Try to extract the last meaningful segment after "Models."
        const string modelsMarker = ".Models.";
        var idx = ns.IndexOf(modelsMarker, StringComparison.OrdinalIgnoreCase);
        if (idx >= 0)
        {
            var segment = ns[(idx + modelsMarker.Length)..];
            var dotIdx = segment.IndexOf('.');
            return (dotIdx > 0 ? segment[..dotIdx] : segment).ToLowerInvariant();
        }

        return entry.Metadata.ClrType.Name.ToLowerInvariant();
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

    private sealed record HospitalAuditEntry(
        string TranCode,
        string UserName,
        string UserAction,
        DateTime ActionDate,
        DateTime ActionTime,
        string? Remarks,
        string? Src,
        string? AuditCat);
}

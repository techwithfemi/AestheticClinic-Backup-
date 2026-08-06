using System.Collections.Generic;

namespace AestheticEMR.Core.Services.Audit;

/// <summary>
/// Helper class for building audit payloads with consistent CRUD patterns.
/// Follows the three-column audit trail strategy:
/// - UserAction: New values (or minimal info for deletes)
/// - OriginalAction: Old values (before update/delete)
/// - Remarks: Human-readable operation description
/// </summary>
public static class AuditPayloadHelper
{
    /// <summary>
    /// Creates audit payloads for a CREATE operation.
    /// OriginalAction is NULL (no previous values existed).
    /// </summary>
    public static (IReadOnlyDictionary<string, object?> payload, IReadOnlyDictionary<string, object?>? original) BuildCreatePayload(
        IReadOnlyDictionary<string, object?> newValues)
    {
        return (newValues, originalPayload: null);
    }

    /// <summary>
    /// Creates audit payloads for an UPDATE operation.
    /// Captures BOTH old and new values for change tracking.
    /// </summary>
    public static (IReadOnlyDictionary<string, object?> payload, IReadOnlyDictionary<string, object?>? original) BuildUpdatePayload(
        IReadOnlyDictionary<string, object?> newValues,
        IReadOnlyDictionary<string, object?> oldValues)
    {
        return (newValues, oldValues);
    }

    /// <summary>
    /// Creates audit payloads for a DELETE operation.
    /// UserAction contains minimal info (just ID).
    /// OriginalAction contains full deleted record (for compliance/recovery).
    /// </summary>
    public static (IReadOnlyDictionary<string, object?> payload, IReadOnlyDictionary<string, object?>? original) BuildDeletePayload(
        IReadOnlyDictionary<string, object?> deletedRecord,
        string idKey = "id")
    {
        // Extract just the ID for UserAction
        var minimalPayload = new Dictionary<string, object?>();
        if (deletedRecord.TryGetValue(idKey, out var idValue))
        {
            minimalPayload[idKey] = idValue;
        }

        return (minimalPayload, deletedRecord);
    }
}

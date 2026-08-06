using AestheticEMR.Core.Services.Account;
using DataAccess.DbAccess;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace AestheticEMR.Core.Infrastructure;

public sealed class HospitalAuditWriter(
    ISqlDataAccess db,
    IUserIdAccessor userIdAccessor,
    ILogger<HospitalAuditWriter> logger) : IHospitalAuditWriter
{
    private const string AuditConnectionId = "DefaultConnection";

    private const string InsertSql =
        "INSERT INTO Auditrail (TranCode, UserName, UserAction, OriginalAction, ActionDate, ActionTime, Remarks, Src, AuditCat) " +
        "VALUES (@TranCode, @UserName, @UserAction, @OriginalAction, @ActionDate, @ActionTime, @Remarks, @Src, @AuditCat);";

    public async Task WriteAsync(string tranCode, string eventType, string src, string auditCat,
        IReadOnlyDictionary<string, object?> payload,
        IReadOnlyDictionary<string, object?>? originalPayload = null)
    {
        try
        {
            var localToday = DateTime.Now.Date;
            var localNow = DateTime.Now;
            var userName = Truncate(userIdAccessor.GetCurrentUserId() ?? "SYSTEM", 50);
            var tc = Truncate(string.IsNullOrWhiteSpace(tranCode) ? "GENERAL" : tranCode, 50);

            // UserAction: JSON object with new values
            var userAction = Truncate(JsonSerializer.Serialize(payload), 5000);
            
            // OriginalAction: JSON object with old values (for Update operations)
            var originalAction = originalPayload != null 
                ? Truncate(JsonSerializer.Serialize(originalPayload), 5000) 
                : null;

            // Remarks: type of CRUD operation
            var remarks = eventType switch
            {
                "Create" => "created record",
                "Update" => $"updated record with priKey: {tc}",
                "Delete" => $"deleted record with priKey: {tc}",
                _ => $"{eventType} record"
            };

            await db.SaveDataText(InsertSql, new
            {
                TranCode = tc,
                UserName = userName,
                UserAction = userAction,
                OriginalAction = originalAction,
                ActionDate = localToday,
                ActionTime = localNow,
                Remarks = Truncate(remarks, 8000),
                Src = Truncate(src, 150),
                AuditCat = Truncate(auditCat, 1000)
            }, AuditConnectionId);
        }
        catch (Exception ex)
        {
            // Audit failure must never fail the business operation — log and swallow
            logger.LogWarning(ex,
                "HospitalAuditWriter failed for TranCode='{TranCode}' eventType='{EventType}'",
                tranCode, eventType);
        }
    }

    private static string Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value)) return string.Empty;
        var trimmed = value.Trim();
        return trimmed.Length <= maxLength ? trimmed : trimmed[..maxLength];
    }
}

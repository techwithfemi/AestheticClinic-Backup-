using AestheticEMR.Core.Services.Legacy.Interfaces;
using AestheticEMR.Core.Services.Legacy.Models;
using Dapper;
using DataAccess.DbAccess;
using Microsoft.Extensions.Logging;

namespace AestheticEMR.Core.Services.Legacy;

public class ShiftDetailService(ISqlDataAccess db, ILogger<ShiftDetailService> logger) : IShiftDetailService
{
    private const string ConnectionId = "smartHRConnection";

    public async Task<IEnumerable<ShiftDetailItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        const string sql = @"
SELECT
    CAST([ShiftID] AS bigint) AS ShiftId,
    LTRIM(RTRIM(ISNULL([Shift/Job], ''))) AS ShiftJob,
    LTRIM(RTRIM(ISNULL([ShiftName], ''))) AS PeriodOfDay,
    LTRIM(RTRIM(ISNULL(CONVERT(varchar(20), [ResumptionTime], 120), ''))) AS ResumptionTime,
    LTRIM(RTRIM(ISNULL(CONVERT(varchar(20), [ClosingTime], 120), ''))) AS ClosingTime,
    LTRIM(RTRIM(ISNULL([EarlyResumptionRemarks], ''))) AS PunctualityRemarks,
    LTRIM(RTRIM(ISNULL([LateResumptionRemarks], ''))) AS LateRemarks,
    LTRIM(RTRIM(ISNULL([NormalClosingRemarks], ''))) AS NormalClosingRemarks,
    LTRIM(RTRIM(ISNULL([AbnormalClosingRemarks], ''))) AS AbnormalClosingRemarks,
    LTRIM(RTRIM(ISNULL([evalTo], ''))) AS EvalTo
FROM qryEmpAttendanceParam
ORDER BY [ShiftID];";

        return await db.LoadDataText<ShiftDetailItem, dynamic>(sql, new { }, ConnectionId);
    }

    public async Task<ShiftDetailItem?> GetByIdAsync(long shiftId, CancellationToken cancellationToken = default)
    {
        const string sql = @"
SELECT TOP 1
    CAST([ShiftID] AS bigint) AS ShiftId,
    LTRIM(RTRIM(ISNULL([Shift/Job], ''))) AS ShiftJob,
    LTRIM(RTRIM(ISNULL([ShiftName], ''))) AS PeriodOfDay,
    LTRIM(RTRIM(ISNULL(CONVERT(varchar(20), [ResumptionTime], 120), ''))) AS ResumptionTime,
    LTRIM(RTRIM(ISNULL(CONVERT(varchar(20), [ClosingTime], 120), ''))) AS ClosingTime,
    LTRIM(RTRIM(ISNULL([EarlyResumptionRemarks], ''))) AS PunctualityRemarks,
    LTRIM(RTRIM(ISNULL([LateResumptionRemarks], ''))) AS LateRemarks,
    LTRIM(RTRIM(ISNULL([NormalClosingRemarks], ''))) AS NormalClosingRemarks,
    LTRIM(RTRIM(ISNULL([AbnormalClosingRemarks], ''))) AS AbnormalClosingRemarks,
    LTRIM(RTRIM(ISNULL([evalTo], ''))) AS EvalTo
FROM qryEmpAttendanceParam
WHERE [ShiftID] = @ShiftId;";

        return (await db.LoadDataText<ShiftDetailItem, dynamic>(sql, new { ShiftId = shiftId }, ConnectionId)).FirstOrDefault();
    }

    public async Task<IEnumerable<ShiftLookupItem>> GetShiftLookupsAsync(CancellationToken cancellationToken = default)
    {
        const string sql = @"
SELECT DISTINCT
    CAST(SNo AS bigint) AS ShiftId,
    ShiftName AS ShiftJob
FROM EmpAttendanceShift;";

        return await db.LoadDataText<ShiftLookupItem, dynamic>(sql, new { }, ConnectionId);
    }

    public async Task<ShiftDetailItem> CreateAsync(ShiftDetailItem item, string currentUserName, CancellationToken cancellationToken = default)
    {
        Validate(item);

        const string sql = @"
INSERT INTO empAttendanceParam
(shifttype, resumTime, closeTime, resumremearly, resumremLate, closeRemNorm, closeRemAbNorm, EvalTo, ShiftID)
VALUES
(@ShiftJob, @ResumptionTime, @ClosingTime, @PunctualityRemarks, @LateRemarks, @NormalClosingRemarks, @AbnormalClosingRemarks, @EvalTo, @ShiftId);";

        await db.SaveDataText(sql, new
        {
            ShiftId = item.ShiftId,
            ShiftJob = item.ShiftJob.Trim(),
            ResumptionTime = item.ResumptionTime,
            ClosingTime = item.ClosingTime,
            PunctualityRemarks = string.IsNullOrWhiteSpace(item.PunctualityRemarks) ? "OK" : item.PunctualityRemarks.Trim(),
            LateRemarks = string.IsNullOrWhiteSpace(item.LateRemarks) ? "OK" : item.LateRemarks.Trim(),
            NormalClosingRemarks = item.NormalClosingRemarks?.Trim(),
            AbnormalClosingRemarks = string.IsNullOrWhiteSpace(item.AbnormalClosingRemarks) ? "OK" : item.AbnormalClosingRemarks.Trim(),
            EvalTo = (item.EvalTo ?? item.PeriodOfDay)?.Trim()
        }, ConnectionId);

        logger.LogInformation("Created shift detail {ShiftJob} by {User}", item.ShiftJob, currentUserName);
        var created = await GetByIdAsync(item.ShiftId, cancellationToken);
        return created ?? item;
    }

    public async Task<ShiftDetailItem> UpdateAsync(ShiftDetailItem item, string currentUserName, CancellationToken cancellationToken = default)
    {
        Validate(item);

        logger.LogInformation("Updating shift {ShiftId}: ResumptionTime={ResumptionTime}, ClosingTime={ClosingTime}", 
            item.ShiftId, item.ResumptionTime, item.ClosingTime);

        const string sql = @"
UPDATE empAttendanceParam
SET shifttype = @ShiftJob,
    resumTime = @ResumptionTime,
    closeTime = @ClosingTime,
    resumremearly = @PunctualityRemarks,
    resumremLate = @LateRemarks,
    closeRemNorm = @NormalClosingRemarks,
    closeRemAbNorm = @AbnormalClosingRemarks,
    EvalTo = @EvalTo
WHERE ShiftID = @ShiftId;";

        await db.SaveDataText(sql, new
        {
            ShiftId = item.ShiftId,
            ShiftJob = item.ShiftJob.Trim(),
            ResumptionTime = item.ResumptionTime,
            ClosingTime = item.ClosingTime,
            PunctualityRemarks = string.IsNullOrWhiteSpace(item.PunctualityRemarks) ? "OK" : item.PunctualityRemarks.Trim(),
            LateRemarks = string.IsNullOrWhiteSpace(item.LateRemarks) ? "OK" : item.LateRemarks.Trim(),
            NormalClosingRemarks = item.NormalClosingRemarks?.Trim(),
            AbnormalClosingRemarks = string.IsNullOrWhiteSpace(item.AbnormalClosingRemarks) ? "OK" : item.AbnormalClosingRemarks.Trim(),
            EvalTo = (item.EvalTo ?? item.PeriodOfDay)?.Trim()
        }, ConnectionId);

        logger.LogInformation("Updated shift detail {ShiftId} by {User}", item.ShiftId, currentUserName);
        var updated = await GetByIdAsync(item.ShiftId, cancellationToken);
        
        if (updated != null)
        {
            logger.LogInformation("After update read from DB: ResumptionTime={ResumptionTime}, ClosingTime={ClosingTime}", 
                updated.ResumptionTime, updated.ClosingTime);
        }
        
        return updated ?? item;
    }

    public async Task<bool> DeleteAsync(long shiftId, string currentUserName, CancellationToken cancellationToken = default)
    {
        const string sql = "DELETE FROM empAttendanceParam WHERE ShiftID = @ShiftId;";
        await db.SaveDataText(sql, new { ShiftId = shiftId }, ConnectionId);
        logger.LogInformation("Deleted shift detail {ShiftId} by {User}", shiftId, currentUserName);
        return true;
    }

    private static void Validate(ShiftDetailItem item)
    {
        if (item.ShiftId <= 0)
        {
            throw new InvalidOperationException("Shift is required.");
        }

        if (string.IsNullOrWhiteSpace(item.ShiftJob))
        {
            throw new InvalidOperationException("Shift/Job is required.");
        }

        if (string.IsNullOrWhiteSpace(item.PeriodOfDay))
        {
            throw new InvalidOperationException("Period of Day is required.");
        }

        if (string.IsNullOrWhiteSpace(item.ResumptionTime))
        {
            throw new InvalidOperationException("Resumption Time is required.");
        }

        if (string.IsNullOrWhiteSpace(item.ClosingTime))
        {
            throw new InvalidOperationException("Closing Time is required.");
        }
    }
}

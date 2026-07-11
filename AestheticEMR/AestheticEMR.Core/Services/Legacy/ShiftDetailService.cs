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
    CAST(ShiftID AS bigint) AS ShiftId,
    LTRIM(RTRIM(ISNULL([Shift/Job], ''))) AS ShiftJob,
    LTRIM(RTRIM(ISNULL(EvalTo, ''))) AS PeriodOfDay,
    CONVERT(varchar(5), resumTime, 108) AS ResumptionTime,
    CONVERT(varchar(5), closeTime, 108) AS ClosingTime,
    LTRIM(RTRIM(ISNULL(resumremearly, ''))) AS PunctualityRemarks,
    LTRIM(RTRIM(ISNULL(resumremLate, ''))) AS LateRemarks,
    LTRIM(RTRIM(ISNULL(closeRemNorm, ''))) AS NormalClosingRemarks,
    LTRIM(RTRIM(ISNULL(closeRemAbNorm, ''))) AS AbnormalClosingRemarks,
    LTRIM(RTRIM(ISNULL(EvalTo, ''))) AS EvalTo
FROM qryEmpAttendanceParam
ORDER BY [Shift/Job];";

        return await db.LoadDataText<ShiftDetailItem, dynamic>(sql, new { }, ConnectionId);
    }

    public async Task<ShiftDetailItem?> GetByIdAsync(long shiftId, CancellationToken cancellationToken = default)
    {
        const string sql = @"
SELECT TOP 1
    CAST(ShiftID AS bigint) AS ShiftId,
    LTRIM(RTRIM(ISNULL([Shift/Job], ''))) AS ShiftJob,
    LTRIM(RTRIM(ISNULL(EvalTo, ''))) AS PeriodOfDay,
    CONVERT(varchar(5), resumTime, 108) AS ResumptionTime,
    CONVERT(varchar(5), closeTime, 108) AS ClosingTime,
    LTRIM(RTRIM(ISNULL(resumremearly, ''))) AS PunctualityRemarks,
    LTRIM(RTRIM(ISNULL(resumremLate, ''))) AS LateRemarks,
    LTRIM(RTRIM(ISNULL(closeRemNorm, ''))) AS NormalClosingRemarks,
    LTRIM(RTRIM(ISNULL(closeRemAbNorm, ''))) AS AbnormalClosingRemarks,
    LTRIM(RTRIM(ISNULL(EvalTo, ''))) AS EvalTo
FROM empAttendanceParam
WHERE ShiftID = @ShiftId;";

        return (await db.LoadDataText<ShiftDetailItem, dynamic>(sql, new { ShiftId = shiftId }, ConnectionId)).FirstOrDefault();
    }

    public async Task<IEnumerable<ShiftLookupItem>> GetShiftLookupsAsync(CancellationToken cancellationToken = default)
    {
        const string sql = @"
SELECT DISTINCT
    CAST(SNo AS bigint) AS ShiftId,
    LTRIM(RTRIM(ShiftName)) AS ShiftJob
FROM EmpAttendanceShift
ORDER BY ShiftName;";

        return await db.LoadDataText<ShiftLookupItem, dynamic>(sql, new { }, ConnectionId);
    }

    public async Task<ShiftDetailItem> CreateAsync(ShiftDetailItem item, string currentUserName, CancellationToken cancellationToken = default)
    {
        Validate(item);

        const string sql = @"
INSERT INTO empAttendanceParam
(ShiftID, [Shift/Job], resumTime, closeTime, resumremearly, resumremLate, closeRemNorm, closeRemAbNorm, EvalTo)
VALUES
(@ShiftId, @ShiftJob, @ResumptionTime, @ClosingTime, @PunctualityRemarks, @LateRemarks, @NormalClosingRemarks, @AbnormalClosingRemarks, @EvalTo);";

        await db.SaveDataText(sql, new
        {
            ShiftId = item.ShiftId,
            ShiftJob = item.ShiftJob.Trim(),
            ResumptionTime = item.ResumptionTime,
            ClosingTime = item.ClosingTime,
            PunctualityRemarks = item.PunctualityRemarks?.Trim(),
            LateRemarks = item.LateRemarks?.Trim(),
            NormalClosingRemarks = item.NormalClosingRemarks?.Trim(),
            AbnormalClosingRemarks = item.AbnormalClosingRemarks?.Trim(),
            EvalTo = item.EvalTo?.Trim()
        }, ConnectionId);

        logger.LogInformation("Created shift detail {ShiftId} by {User}", item.ShiftId, currentUserName);
        var created = await GetByIdAsync(item.ShiftId, cancellationToken);
        return created ?? item;
    }

    public async Task<ShiftDetailItem> UpdateAsync(ShiftDetailItem item, string currentUserName, CancellationToken cancellationToken = default)
    {
        Validate(item);

        const string sql = @"
UPDATE empAttendanceParam
SET [Shift/Job] = @ShiftJob,
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
            PunctualityRemarks = item.PunctualityRemarks?.Trim(),
            LateRemarks = item.LateRemarks?.Trim(),
            NormalClosingRemarks = item.NormalClosingRemarks?.Trim(),
            AbnormalClosingRemarks = item.AbnormalClosingRemarks?.Trim(),
            EvalTo = item.EvalTo?.Trim()
        }, ConnectionId);

        logger.LogInformation("Updated shift detail {ShiftId} by {User}", item.ShiftId, currentUserName);
        var updated = await GetByIdAsync(item.ShiftId, cancellationToken);
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

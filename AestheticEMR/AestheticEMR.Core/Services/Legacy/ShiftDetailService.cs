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
    LTRIM(RTRIM(ISNULL(ShiftName, ''))) AS PeriodOfDay,
    LTRIM(RTRIM(ISNULL([ResumptionTime], ''))) AS ResumptionTime,
    LTRIM(RTRIM(ISNULL([ClosingTime], ''))) AS ClosingTime,
    LTRIM(RTRIM(ISNULL([EarlyResumptionRemarks], ''))) AS PunctualityRemarks,
    LTRIM(RTRIM(ISNULL([LateResumptionRemarks], ''))) AS LateRemarks,
    LTRIM(RTRIM(ISNULL([NormalClosingRemarks], ''))) AS NormalClosingRemarks,
    LTRIM(RTRIM(ISNULL([AbnormalClosingRemarks], ''))) AS AbnormalClosingRemarks,
    LTRIM(RTRIM(ISNULL(evalTo, ''))) AS EvalTo
FROM qryEmpAttendanceParam
ORDER BY ShiftName;";

        return await db.LoadDataText<ShiftDetailItem, dynamic>(sql, new { }, ConnectionId);
    }

    public async Task<ShiftDetailItem?> GetByIdAsync(long shiftId, CancellationToken cancellationToken = default)
    {
        const string sql = @"
SELECT TOP 1
    CAST(ShiftID AS bigint) AS ShiftId,
    LTRIM(RTRIM(ISNULL([Shift/Job], ''))) AS ShiftJob,
    LTRIM(RTRIM(ISNULL(ShiftName, ''))) AS PeriodOfDay,
    LTRIM(RTRIM(ISNULL([ResumptionTime], ''))) AS ResumptionTime,
    LTRIM(RTRIM(ISNULL([ClosingTime], ''))) AS ClosingTime,
    LTRIM(RTRIM(ISNULL([EarlyResumptionRemarks], ''))) AS PunctualityRemarks,
    LTRIM(RTRIM(ISNULL([LateResumptionRemarks], ''))) AS LateRemarks,
    LTRIM(RTRIM(ISNULL([NormalClosingRemarks], ''))) AS NormalClosingRemarks,
    LTRIM(RTRIM(ISNULL([AbnormalClosingRemarks], ''))) AS AbnormalClosingRemarks,
    LTRIM(RTRIM(ISNULL(evalTo, ''))) AS EvalTo
FROM qryEmpAttendanceParam
WHERE ShiftID = @ShiftId;";

        return (await db.LoadDataText<ShiftDetailItem, dynamic>(sql, new { ShiftId = shiftId }, ConnectionId)).FirstOrDefault();
    }

    public async Task<IEnumerable<ShiftLookupItem>> GetShiftLookupsAsync(CancellationToken cancellationToken = default)
    {
        const string sql = @"
SELECT
    CAST(ShiftID AS bigint) AS ShiftId,
    LTRIM(RTRIM(ShiftName)) AS ShiftName
FROM qryEmpAttendanceParam
GROUP BY ShiftID, ShiftName
ORDER BY ShiftName;";

        return await db.LoadDataText<ShiftLookupItem, dynamic>(sql, new { }, ConnectionId);
    }

    public async Task<ShiftDetailItem> CreateAsync(ShiftDetailItem item, string currentUserName, CancellationToken cancellationToken = default)
    {
        Validate(item);

        const string sql = @"
INSERT INTO empAttendanceParam
([Shift/Job], [ResumptionTime], [ClosingTime], [EarlyResumptionRemarks], [LateResumptionRemarks], [NormalClosingRemarks], [AbnormalClosingRemarks], evalTo)
VALUES
(@ShiftJob, @ResumptionTime, @ClosingTime, @PunctualityRemarks, @LateRemarks, @NormalClosingRemarks, @AbnormalClosingRemarks, @EvalTo);";

        await db.SaveDataText(sql, new
        {
            ShiftJob = item.ShiftJob.Trim(),
            ResumptionTime = item.ResumptionTime,
            ClosingTime = item.ClosingTime,
            PunctualityRemarks = item.PunctualityRemarks?.Trim(),
            LateRemarks = item.LateRemarks?.Trim(),
            NormalClosingRemarks = item.NormalClosingRemarks?.Trim(),
            AbnormalClosingRemarks = item.AbnormalClosingRemarks?.Trim(),
            EvalTo = item.EvalTo?.Trim()
        }, ConnectionId);

        logger.LogInformation("Created shift detail {ShiftJob} by {User}", item.ShiftJob, currentUserName);
        var created = await GetAllAsync(cancellationToken);
        return created.FirstOrDefault() ?? item;
    }

    public async Task<ShiftDetailItem> UpdateAsync(ShiftDetailItem item, string currentUserName, CancellationToken cancellationToken = default)
    {
        Validate(item);

        const string sql = @"
UPDATE empAttendanceParam
SET [Shift/Job] = @ShiftJob,
    [ResumptionTime] = @ResumptionTime,
    [ClosingTime] = @ClosingTime,
    [EarlyResumptionRemarks] = @PunctualityRemarks,
    [LateResumptionRemarks] = @LateRemarks,
    [NormalClosingRemarks] = @NormalClosingRemarks,
    [AbnormalClosingRemarks] = @AbnormalClosingRemarks,
    evalTo = @EvalTo
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

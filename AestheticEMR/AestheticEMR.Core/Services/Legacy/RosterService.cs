using AestheticEMR.Core.Services.Account;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using AestheticEMR.Core.Services.Legacy.Models;
using Dapper;
using DataAccess.DbAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace AestheticEMR.Core.Services.Legacy;

public class RosterService(
    ISqlDataAccess db,
    IEmrAppDefaultsService defaultsService,
    IUserIdAccessor userIdAccessor,
    IConfiguration configuration,
    ILogger<RosterService> logger) : IRosterService
{
    private const string SmartHRConnection = "smartHRConnection";
    private const string RosterView = "vwRosterForGridLatest";

    public async Task<RosterLookups> GetLookupsAsync(CancellationToken cancellationToken = default)
    {
        var groups = await db.LoadDataText<RosterGroupLookup, dynamic>(@"
SELECT DISTINCT CAST(SNo AS bigint) AS GroupId,
       LTRIM(RTRIM(GroupName)) AS GroupName,
       LTRIM(RTRIM(DeptID)) AS DeptId,
       LTRIM(RTRIM(DeptName)) AS DeptName
FROM vwRosterGroupAssignedToStaff
ORDER BY GroupName;", new { }, SmartHRConnection);

        var sourceStaff = await db.LoadDataText<RosterStaffLookup, dynamic>(@"
SELECT DISTINCT LTRIM(RTRIM(EmpID)) AS EmpId,
       LTRIM(RTRIM(empFullname)) AS EmpName
FROM vwRosterEmployees
ORDER BY EmpName;", new { }, SmartHRConnection);

        var targetStaff = await db.LoadDataText<RosterStaffLookup, dynamic>(@"
SELECT DISTINCT LTRIM(RTRIM(EmpID)) AS EmpId,
       LTRIM(RTRIM(empFullname)) AS EmpName
FROM qryEmp
WHERE empID NOT IN (
    SELECT DISTINCT EmpID FROM vwRosterEmployees
)
ORDER BY EmpName;", new { }, SmartHRConnection);

        var shifts = await db.LoadDataText<RosterShiftLookup, dynamic>(@"
SELECT DISTINCT CAST(ShiftID AS bigint) AS SNo,
       LTRIM(RTRIM(ShiftName)) AS ShiftName,
       LTRIM(RTRIM(EvalTo)) AS EvalTo,
       LTRIM(RTRIM(DeptID)) AS DeptId
FROM vwEmpDeptShifts
ORDER BY ShiftName;", new { }, SmartHRConnection);

        return new RosterLookups
        {
            Groups = groups.ToList(),
            SourceStaff = sourceStaff.ToList(),
            TargetStaff = targetStaff.ToList(),
            Shifts = shifts.ToList()
        };
    }

    public async Task<IEnumerable<RosterGridItem>> GetGridAsync(RosterGridQuery query, CancellationToken cancellationToken = default)
    {
        var whereParts = new List<string>();
        var param = new DynamicParameters();

        if (query.GroupId.HasValue)
        {
            whereParts.Add("GroupID = @GroupId");
            param.Add("GroupId", query.GroupId.Value);
        }

        if (query.FromDate.HasValue)
        {
            whereParts.Add("[Date] >= @FromDate");
            param.Add("FromDate", query.FromDate.Value.ToDateTime(TimeOnly.MinValue));
        }

        if (query.ToDate.HasValue)
        {
            whereParts.Add("[Date] <= @ToDate");
            param.Add("ToDate", query.ToDate.Value.ToDateTime(TimeOnly.MaxValue));
        }

        var where = whereParts.Count == 0 ? string.Empty : "WHERE " + string.Join(" AND ", whereParts);
        var rows = await db.LoadDataText<RosterGridItem, DynamicParameters>($@"
SELECT SNo, [Date], StaffName, ClockIn, ClockOut, Status, Fine, ShiftName, GroupName, StartDate, EndDate,
       DeptName, Exempted, GroupID, RosterGrpShiftID, EmpID, ShiftAbbrv
FROM {RosterView}
{where}
ORDER BY [Date], StaffName;", param, SmartHRConnection);

        return rows;
    }

    public async Task<IEnumerable<RosterGridItem>> GetExistingAsync(RosterEditorQuery query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query.EmpId))
        {
            return [];
        }

        var deptId = await ResolveDeptIdAsync(query.DeptId, cancellationToken);
        var param = new DynamicParameters();
        param.Add("EmpId", query.EmpId.Trim());
        param.Add("DeptId", deptId);
        if (query.FromDate.HasValue)
        {
            param.Add("FromDate", query.FromDate.Value.ToDateTime(TimeOnly.MinValue));
        }
        if (query.ToDate.HasValue)
        {
            param.Add("ToDate", query.ToDate.Value.ToDateTime(TimeOnly.MaxValue));
        }

        var whereDate = query.FromDate.HasValue && query.ToDate.HasValue
            ? "AND RosterDate BETWEEN @FromDate AND @ToDate"
            : query.FromDate.HasValue
                ? "AND RosterDate >= @FromDate"
                : query.ToDate.HasValue ? "AND RosterDate <= @ToDate" : string.Empty;

        var rows = await db.LoadDataText<RosterGridItem, DynamicParameters>($@"
SELECT CAST(SNo AS bigint) AS SNo,
       RosterDate AS [Date],
       StaffName,
       NULL AS ClockIn,
       NULL AS ClockOut,
       NULL AS Status,
       CAST(0 AS decimal(18, 2)) AS Fine,
       ShiftName,
       GroupName,
       StartDate,
       EndDate,
       DeptName,
       Exempted,
       CAST(GroupID AS nvarchar(50)) AS GroupID,
       CAST(RosterGrpShiftID AS bigint) AS RosterGrpShiftID,
       EmpID,
       ShiftAbbrv
FROM Roster
WHERE EmpID = @EmpId
  AND DeptID = @DeptId {whereDate}
ORDER BY RosterDate;", param, SmartHRConnection);

        return rows;
    }

    public async Task<RosterSaveResult> SaveAsync(RosterSaveRequest request, string currentUserName, CancellationToken cancellationToken = default)
    {
        if (request.SelectedDays.Count == 0)
        {
            throw new InvalidOperationException("At least one day is required.");
        }

        var defaults = await defaultsService.GetAsync(cancellationToken);
        var enabled = defaults.Get("Roster_Enabled", "YES");
        if (!enabled.Equals("YES", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Roster is disabled.");
        }

        var deptId = await ResolveDeptIdAsync(request.DeptId, cancellationToken);
        var rosterGroupId = request.GroupId ?? 0;
        if (rosterGroupId <= 0)
        {
            throw new InvalidOperationException("Group is required.");
        }

        var groupName = request.GroupName.Trim();
        var offDutyShiftId = defaults.Get("Roster_OFF_DUTY_ShiftID", string.Empty);
        var leaveShiftId = defaults.Get("Roster_LEAVE_ShiftID", string.Empty);

        var minSelected = request.SelectedDays.Min(x => x.Date);
        var monthStart = new DateOnly(minSelected.Year, minSelected.Month, 1);
        var monthEnd = new DateOnly(minSelected.Year, minSelected.Month, DateTime.DaysInMonth(minSelected.Year, minSelected.Month));

        var connectionString = configuration.GetConnectionString(SmartHRConnection)
            ?? throw new InvalidOperationException($"Connection string '{SmartHRConnection}' was not found.");

        string? targetEmpId;

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            targetEmpId = await connection.QueryFirstOrDefaultAsync<string>(@"
SELECT TOP 1 LTRIM(RTRIM(EmpID))
FROM Employees
WHERE LTRIM(RTRIM(CAST(RosterGrpID AS nvarchar(50)))) = @GroupId
ORDER BY EmpID;",
                new { GroupId = rosterGroupId.ToString() }, transaction);

            if (string.IsNullOrWhiteSpace(targetEmpId))
            {
                throw new InvalidOperationException("No Employees in this group to save.");
            }

            targetEmpId = targetEmpId.Trim();

            await connection.ExecuteAsync(@"
DELETE FROM Roster
WHERE RosterDate BETWEEN @StartDate AND @EndDate
  AND GroupID = @GroupID;",
                new
                {
                    StartDate = monthStart.ToDateTime(TimeOnly.MinValue),
                    EndDate = monthEnd.ToDateTime(TimeOnly.MaxValue),
                    GroupID = rosterGroupId
                }, transaction);

            foreach (var day in request.SelectedDays.OrderBy(x => x.Date))
            {
                var isOffDuty = day.ShiftId.ToString().Equals(offDutyShiftId, StringComparison.OrdinalIgnoreCase)
                    || day.ShiftId.ToString().Equals(leaveShiftId, StringComparison.OrdinalIgnoreCase);

                await connection.ExecuteAsync(@"
DELETE FROM Roster
WHERE RosterDate = @RosterDate
  AND EmpID = @EmpID;",
                    new
                    {
                        RosterDate = day.Date.ToDateTime(TimeOnly.MinValue),
                        EmpID = targetEmpId
                    }, transaction);

                await connection.ExecuteAsync(@"
INSERT INTO Roster
(RosterGrpShiftID, EmpID, ShiftID, GroupID, isOffDuty, ShiftAbbrv, ShiftName, GroupName, DeptID, RosterDate)
VALUES
(@RosterGrpShiftID, @EmpID, @ShiftID, @GroupID, @IsOffDuty, @ShiftAbbrv, @ShiftName, @GroupName, @DeptID, @RosterDate);",
                    new
                    {
                        RosterGrpShiftID = 0,
                        EmpID = targetEmpId,
                        ShiftID = day.ShiftId,
                        GroupID = rosterGroupId,
                        IsOffDuty = isOffDuty ? 1 : 0,
                        ShiftAbbrv = day.ShiftAbbrv,
                        ShiftName = day.ShiftName,
                        GroupName = groupName,
                        DeptID = deptId,
                        RosterDate = day.Date.ToDateTime(TimeOnly.MinValue)
                    }, transaction);
            }

            var selectedDateSet = request.SelectedDays.Select(x => x.Date).ToHashSet();
            for (var date = monthStart; date <= monthEnd; date = date.AddDays(1))
            {
                if (selectedDateSet.Contains(date))
                {
                    continue;
                }

                await connection.ExecuteAsync(@"
DELETE FROM Roster
WHERE RosterDate = @RosterDate
  AND EmpID = @EmpID;",
                    new
                    {
                        RosterDate = date.ToDateTime(TimeOnly.MinValue),
                        EmpID = targetEmpId
                    }, transaction);

                await connection.ExecuteAsync(@"
INSERT INTO Roster
(RosterGrpShiftID, EmpID, ShiftID, GroupID, isOffDuty, ShiftAbbrv, ShiftName, GroupName, DeptID, RosterDate)
VALUES
(@RosterGrpShiftID, @EmpID, @ShiftID, @GroupID, @IsOffDuty, @ShiftAbbrv, @ShiftName, @GroupName, @DeptID, @RosterDate);",
                    new
                    {
                        RosterGrpShiftID = 0,
                        EmpID = targetEmpId,
                        ShiftID = 0,
                        GroupID = rosterGroupId,
                        IsOffDuty = 1,
                        ShiftAbbrv = (string?)null,
                        ShiftName = "PLS_ENTER_SHIFT",
                        GroupName = groupName,
                        DeptID = deptId,
                        RosterDate = date.ToDateTime(TimeOnly.MinValue)
                    }, transaction);
            }

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            logger.LogError(ex, "Error saving roster for group {GroupId}", rosterGroupId);
            throw;
        }

        // Query the saved data AFTER the transaction is closed
        var items = (await GetExistingAsync(new RosterEditorQuery
        {
            EmpId = targetEmpId,
            DeptId = deptId,
            FromDate = monthStart,
            ToDate = monthEnd
        }, cancellationToken)).ToList();

        logger.LogInformation("Saved roster for {TargetEmpId} in group {GroupId} by {User}", targetEmpId, rosterGroupId, currentUserName);
        return new RosterSaveResult
        {
            CreatedCount = items.Count,
            Items = items
        };
    }

    public async Task<bool> DeleteAsync(RosterDeleteRequest request, string currentUserName, CancellationToken cancellationToken = default)
    {
        await db.SaveDataText("DELETE FROM Roster WHERE SNo = @SNo;", new { request.SNo }, SmartHRConnection);
        logger.LogInformation("Deleted roster row {SNo} by {User}", request.SNo, currentUserName);
        return true;
    }

    private async Task<string> ResolveDeptIdAsync(string? deptId, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(deptId))
        {
            return deptId.Trim();
        }

        var empId = userIdAccessor.GetCurrentUserEmpId();
        if (string.IsNullOrWhiteSpace(empId))
        {
            throw new InvalidOperationException("Unable to resolve the current department.");
        }

        var rows = await db.LoadDataText<DeptResolverRow, dynamic>(@"
SELECT TOP 1 LTRIM(RTRIM(DeptID)) AS DeptId
FROM qryEmp
WHERE EmpID = @EmpId;", new { EmpId = empId.Trim() }, SmartHRConnection);

        var resolved = rows.FirstOrDefault()?.DeptId?.Trim();
        if (string.IsNullOrWhiteSpace(resolved))
        {
            throw new InvalidOperationException("Unable to resolve the current department.");
        }

        return resolved;
    }

    private sealed class DeptResolverRow
    {
        public string? DeptId { get; set; }
    }
}

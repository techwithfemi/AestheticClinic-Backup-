# Legacy Crystal Reports Rendering Rules

**Reference this file when extending AestheticClinic to add other legacy Crystal reports to the modern .NET web UI.**

## Quick Summary

Legacy Crystal Reports (`.rpt` files) from the VB.NET Accounting app require exact behavior alignment across three layers:
1. **Angular Frontend** - capture display text + IDs
2. **.NET Backend** - forward display text to legacy service
3. **Legacy Crystal Service** - use display text for report headers; preserve DataSet column types

---

## Frontend Rules (Angular)

### 1. Capture Selected Display Text
**Why**: The legacy Crystal report needs both the **ID/code** AND the **visible dropdown text** that users selected.

- Don't just pass `ledgerCode` and `accountNo` (IDs).
- **Also pass `ledgerDisplayText` and `accountDisplayText`** (the selected combo box text).

**Example**:
```typescript
// Capture display text from the selected item
const ledgerDisplayText = this.selectedGlLedger()?.ledger ?? '';
const accountDisplayText = this.selectedGlAccount()?.accountName ?? '';

this.endpoint.getAccountingGeneralLedgerReportEndpoint({
  coyID: this.coyID.trim(),
  period: this.period,
  ledgerCode: this.ledgerCode,
  accountNo: this.accountNo,
  ledgerDisplayText,  // <-- CRITICAL
  accountDisplayText  // <-- CRITICAL
})
```

### 2. Update the Endpoint Service
**Why**: The HTTP client must include display text in the query string.

- Add optional `displayText` or similar parameters to the endpoint method signature.
- Include them in the `URLSearchParams` when building the request.

**Example**:
```typescript
getReportEndpoint(params: { 
  coyID: string; 
  period: string; 
  code: string; 
  id: string; 
  displayText?: string;    // <-- Add this
}): Observable<Blob> {
  const query = new URLSearchParams({
    coyID: params.coyID,
    period: params.period,
    code: params.code,
    id: params.id
  });

  if (params.displayText) {
    query.set('displayText', params.displayText);  // <-- Include this
  }

  return this.http.get(`${this.reportsUrl}/report?${query.toString()}`, {
    ...this.requestHeaders,
    responseType: 'blob'
  }).pipe(
    catchError(error => this.handleError(error, () => this.getReportEndpoint(params)))
  ) as Observable<Blob>;
}
```

---

## Backend Rules (.NET)

### 3. Accept Display Text in Controller
**Why**: The controller must forward display text to the legacy report proxy service.

- Add optional `displayText` query parameters to the action method signature.
- Pass them through to the proxy service.

**Example**:
```csharp
[HttpGet("general-ledger")]
public async Task<IActionResult> GetGeneralLedger(
    [FromQuery] string coyID, 
    [FromQuery] string period, 
    [FromQuery] string code, 
    [FromQuery] string id,
    [FromQuery] string? displayText,  // <-- Add this
    CancellationToken ct)
{
    try
    {
        var report = await reportProxyService.GetReportAsync(
            coyID.Trim(), 
            period.Trim(), 
            code.Trim(), 
            id.Trim(),
            displayText?.Trim(),  // <-- Pass this
            ct
        );
        return File(report.Content, report.ContentType, report.FileName);
    }
    catch (InvalidOperationException ex)
    {
        AddModelError(ex.Message);
        return BadRequest(new ValidationProblemDetails(ModelState));
    }
}
```

### 4. Forward Display Text in Proxy Service
**Why**: The proxy must include display text in the query string sent to the legacy Crystal service.

- Update the service interface to accept display text parameters.
- Include them in the query dictionary when calling `SendGetAsync()`.

**Example**:
```csharp
public async Task<LegacyCrystalReportPayload> GetReportAsync(
    string coyID, 
    string period, 
    string code, 
    string id, 
    string? displayText,  // <-- Add this
    CancellationToken cancellationToken)
{
    var query = new Dictionary<string, string?>
    {
        ["coyID"] = coyID,
        ["period"] = period,
        ["code"] = code,
        ["id"] = id,
        ["displayText"] = displayText  // <-- Include this
    };

    var response = await SendGetAsync("ReportRoute", query, cancellationToken);
    return await BuildPayloadAsync(response, $"report-{period}.pdf", cancellationToken);
}
```

### 5. Use Display Text for Report Header
**Why**: Legacy Crystal reports used the selected dropdown text for txtPrd/txtHead, not the code.

- In the legacy Crystal controller, build the report header from **display text**, not just IDs.
- Follow VB app logic: if account is not `(ALL)`, use account display; otherwise use ledger display.

**Example**:
```csharp
private static string BuildReportHeader(
    string ledgerCode, 
    string accountNo, 
    string? ledgerDisplayText, 
    string? accountDisplayText)
{
    var acct = accountNo?.Trim();
    if (!string.IsNullOrWhiteSpace(acct) && !string.Equals(acct, "(ALL)", StringComparison.OrdinalIgnoreCase))
    {
        if (!string.IsNullOrWhiteSpace(accountDisplayText))
        {
            return accountDisplayText.Trim();
        }
        return acct;
    }

    if (!string.IsNullOrWhiteSpace(ledgerDisplayText))
    {
        return ledgerDisplayText.Trim();
    }

    return ledgerCode?.Trim() ?? string.Empty;
}
```

### 6. Preserve DataSet Column Types
**Why**: Crystal Reports is sensitive to column types. Loose `object` types cause placeholder values like `1`.

- When converting Dapper rows to DataSet, **detect and preserve real CLR column types**.
- Don't force all columns to `object` type.
- Use `Nullable.GetUnderlyingType()` for nullable types.

**Example**:
```csharp
private static Type GetColumnType(IReadOnlyCollection<IDictionary<string, object>> rows, string columnName)
{
    foreach (var row in rows)
    {
        object value;
        if (row.TryGetValue(columnName, out value) && value != null)
        {
            return Nullable.GetUnderlyingType(value.GetType()) ?? value.GetType();
        }
    }
    return typeof(string);
}

private static DataSet ToDataSet(IEnumerable<dynamic> rows)
{
    var dataSet = new DataSet();
    var dataTable = new DataTable();

    var dictionaries = rows == null
        ? new List<IDictionary<string, object>>()
        : rows.Select(r => r as IDictionary<string, object> ?? new Dictionary<string, object>()).ToList();

    if (dictionaries.Count == 0)
    {
        dataSet.Tables.Add(dataTable);
        return dataSet;
    }

    // Collect all unique column names across all rows
    var columnNames = new List<string>();
    foreach (var row in dictionaries)
    {
        foreach (var key in row.Keys)
        {
            if (!columnNames.Contains(key))
            {
                columnNames.Add(key);
            }
        }
    }

    // Add columns with CORRECT types (not object)
    foreach (var columnName in columnNames)
    {
        dataTable.Columns.Add(columnName, GetColumnType(dictionaries, columnName));
    }

    // Add rows
    foreach (var rowDict in dictionaries)
    {
        var row = dataTable.NewRow();
        foreach (var columnName in columnNames)
        {
            object value;
            if (!rowDict.TryGetValue(columnName, out value) || value == null)
            {
                row[columnName] = DBNull.Value;
            }
            else
            {
                row[columnName] = value;
            }
        }
        dataTable.Rows.Add(row);
    }

    dataSet.Tables.Add(dataTable);
    return dataSet;
}
```

### 7. Handle Legacy Stored Procedure Compatibility
**Why**: Some stored procedures may return inconsistent or extra columns.

- Ensure the stored procedure (e.g., `getGL`) matches the legacy schema exactly.
- If needed, validate that result columns match what the Crystal report template expects.
- Use Dapper's dynamic row mapping to be flexible.

---

## Crystal Report Template Rules

### 8. Report Header Field Binding
**Why**: Legacy reports use fixed text object fields like `txtPrd`, `txtCoy`, etc.

- **`txtPrd`**: Set from display text + date (passed via C# backend).
- **`txtCoy`**: Set from company name.
- **`txtHead`**: Optional secondary header.

Ensure the Crystal report template has these text objects named correctly and in the right sections (report header or page header).

### 9. Group/Section Naming
**Why**: Crystal groups by field names in the dataset.

- The Crystal report must group on actual dataset column names (e.g., `GroupName`, `GroupID`).
- If the report groups on `AccountName` but the stored procedure returns `AcctName`, there will be a mismatch.
- Verify the stored procedure result columns match the report's field references.

---

## Complete Flow Checklist

When adding a new legacy Crystal report to the web UI, follow this sequence:

### Frontend (Angular)
- [ ] Create/update the endpoint method to accept display text params
- [ ] In the component, capture selected dropdown text
- [ ] Pass both IDs and display text to the endpoint call

### Backend Controller
- [ ] Add optional `displayText` query parameters
- [ ] Accept them from the request
- [ ] Pass them to the proxy service

### Proxy Service
- [ ] Update interface to include display text params
- [ ] Include them in the query dictionary sent to legacy service

### Legacy Crystal Controller
- [ ] Accept display text params from query string
- [ ] Build report header from display text (not just IDs)
- [ ] Ensure DataSet columns preserve CLR types
- [ ] Pass header and display text to `CrystalReport.RenderReport()`

### DataSet Conversion
- [ ] Use `GetColumnType()` helper to detect real types
- [ ] Don't force all columns to `object`
- [ ] Preserve nullable types

---

## Example: Adding Another Report

### 1. Angular Endpoint
```typescript
getBalanceSheetReportEndpoint(params: { 
  coyID: string; 
  period: string; 
  year: string; 
  rptBy: string; 
  isClose: boolean;
  reportTypeDisplayText?: string;  // <-- NEW
}): Observable<Blob> {
  const query = new URLSearchParams({
    coyID: params.coyID,
    period: params.period,
    year: params.year,
    rptBy: params.rptBy,
    isClose: String(params.isClose)
  });

  if (params.reportTypeDisplayText) {
    query.set('reportTypeDisplayText', params.reportTypeDisplayText);  // <-- NEW
  }

  return this.http.get(`${this.reportsUrl}/balance-sheet?${query.toString()}`, {
    ...this.requestHeaders,
    responseType: 'blob'
  }).pipe(...) as Observable<Blob>;
}
```

### 2. Component Call
```typescript
this.endpoint.getBalanceSheetReportEndpoint({
  coyID: this.coyID,
  period: this.period,
  year: this.year,
  rptBy: this.rptBy,
  isClose: false,
  reportTypeDisplayText: this.selectedBsHeader()?.itemName ?? ''  // <-- NEW
})
```

### 3. Controller
```csharp
[HttpGet("balance-sheet")]
public async Task<IActionResult> GetBalanceSheet(
    [FromQuery] string coyID,
    [FromQuery] string period,
    [FromQuery] string year,
    [FromQuery] string rptBy,
    [FromQuery] bool isClose,
    [FromQuery] string? reportTypeDisplayText,  // <-- NEW
    CancellationToken ct)
{
    var report = await reportProxyService.GetBalanceSheetReportAsync(
        coyID, period, year, rptBy, isClose, reportTypeDisplayText, ct);  // <-- NEW
    return File(report.Content, report.ContentType, report.FileName);
}
```

### 4. Proxy Service
```csharp
public async Task<LegacyCrystalReportPayload> GetBalanceSheetReportAsync(
    string coyID, 
    string period, 
    string year, 
    string rptBy, 
    bool isClose,
    string? reportTypeDisplayText,  // <-- NEW
    CancellationToken cancellationToken)
{
    var query = new Dictionary<string, string?>
    {
        ["coyID"] = coyID,
        ["period"] = period,
        ["year"] = year,
        ["rptBy"] = rptBy,
        ["isClose"] = isClose.ToString().ToLowerInvariant(),
        ["reportTypeDisplayText"] = reportTypeDisplayText  // <-- NEW
    };

    var response = await SendGetAsync("Accounting/BalanceSheet", query, cancellationToken);
    return await BuildPayloadAsync(response, $"balance-sheet-{period}.pdf", cancellationToken);
}
```

### 5. Legacy Crystal Controller
```csharp
[Route("Accounting/BalanceSheet")]
[HttpGet]
public async Task<HttpResponseMessage> BalanceSheet(
    string coyID, 
    string period, 
    string year, 
    string rptBy, 
    bool isClose = false,
    string reportTypeDisplayText = null)  // <-- NEW
{
    // ... validation ...

    var ds = await DapperReportData.ExecuteDataSetAsync(conStr, "getBalanceSheetHeaders", 
        new { CoyID = coyID, period = period, Year = year, PrdType = rptBy }, 600);

    var header = BuildBalanceSheetHeader(
        reportTypeDisplayText ?? rptBy, 
        year, 
        period, 
        isClose, 
        DateTime.Today);  // <-- USE displayText

    return CrystalReport.RenderReport(reportPath, reportFileName, exportFilename, ds, header);
}

private static string BuildBalanceSheetHeader(
    string displayText, 
    string year, 
    string period, 
    bool isClose, 
    DateTime reportDate)
{
    var text = string.IsNullOrWhiteSpace(displayText) ? "Balance Sheet" : displayText;
    if (isClose)
    {
        return $"{text} For Period ended {reportDate.ToShortDateString()}";
    }
    return $"{text} As of {reportDate.ToShortDateString()}";
}
```

---

## Key Takeaways

1. **Legacy reports need display text**: IDs/codes alone are not enough; pass the visible dropdown text.
2. **DataSet column types matter**: Crystal is type-sensitive; preserve real CLR types, don't flatten to `object`.
3. **Header comes from display text**: Use selected combo box text, not just the code.
4. **Stored procedure schema**: Ensure the `getGL`-like procedures return columns that match the Crystal report field references.
5. **Follow the three-layer flow**: Frontend → Backend Controller → Proxy Service → Legacy Crystal Service.
6. **Non-SELECT stored procedures MUST use `ExecuteNonQueryAsync`**: See Rule 10 below.

---

## Rule 10: Non-SELECT Stored Procedures Must Use ExecuteNonQueryAsync

### Problem
Some legacy Crystal reports call a **non-SELECT stored procedure** before fetching the report data — for example `CloseAccountingPeriod` is a DML/side-effect proc that closes a period and returns no result set.

Calling these procs via `DapperReportData.ExecuteDataSetAsync()` (which uses `QueryAsync` internally) will cause the error:

```
System.ArgumentException: Column '' does not belong to table
```

This happens because Dapper's `QueryAsync` on a non-SELECT proc returns a row with a blank column name `""`, which crashes `DataTable.set_Item("")` in `ToDataSet()`.

### Rule
> **Any stored procedure that performs DML or returns no result set MUST be called using `DapperReportData.ExecuteNonQueryAsync()`, NOT `ExecuteDataSetAsync()`.**

### How to Identify Non-SELECT Procs
When porting a VB.NET report, inspect the original form code. If the VB code calls `cmd.ExecuteNonQuery()` for a stored procedure (not `da.Fill(ds)`), that proc is a non-SELECT and must use `ExecuteNonQueryAsync` in the Crystal Web API.

**VB pattern that signals non-SELECT (use `ExecuteNonQueryAsync`)**:
```vb
cmd.CommandText = "CloseAccountingPeriod"
cmd.Parameters.AddWithValue("@Period", ...)
cmd.ExecuteNonQuery()   ' <-- non-SELECT
```

**VB pattern that signals SELECT (use `ExecuteDataSetAsync`)**:
```vb
cmd.CommandText = "getBalanceSheetHeaders"
da.SelectCommand = cmd
da.Fill(ds)             ' <-- SELECT / result set
```

### Correct Usage in CrystalReportWebAPI

```csharp
// ✅ CORRECT: non-SELECT stored proc — use ExecuteNonQueryAsync
await DapperReportData.ExecuteNonQueryAsync(conStr, "CloseAccountingPeriod", new
{
    Period = period.Trim(),
    coyID = coyID.Trim(),
    UserName = string.Empty,
    isClose = 0,
    isBS = 1
}, 600);

// ✅ CORRECT: SELECT stored proc — use ExecuteDataSetAsync
var ds = await DapperReportData.ExecuteDataSetAsync(conStr, "getBalanceSheetHeaders", new
{
    CoyID = coyID.Trim(),
    period = period.Trim(),
    Year = year.Trim(),
    PrdType = rptBy.Trim()
}, 600);
```
```csharp
// ❌ WRONG: calling a non-SELECT proc with ExecuteDataSetAsync crashes with Column '' error
await DapperReportData.ExecuteDataSetAsync(conStr, "CloseAccountingPeriod", new { ... }, 600);
```

### Checklist When Adding a New Legacy Report

- [ ] Review the VB.NET form code for ALL `cmd.ExecuteNonQuery()` calls before `da.Fill(ds)`
- [ ] For each non-SELECT proc, use `DapperReportData.ExecuteNonQueryAsync()`
- [ ] For each SELECT/result-set proc, use `DapperReportData.ExecuteDataSetAsync()`
- [ ] Never call `ExecuteDataSetAsync` on `CloseAccountingPeriod` or any similar side-effect proc

### Known Non-SELECT Procs (confirmed in this codebase)

| Stored Procedure        | Used In                                   | Call With              |
|-------------------------|-------------------------------------------|------------------------|
| `CloseAccountingPeriod` | BalanceSheet, ProfitAndLoss, P&L Details  | `ExecuteNonQueryAsync` |

Add new entries to this table as you port more VB.NET reports.

---

## Files Modified for Balance Sheet Report (Reference)

- `AestheticEMR/AestheticEMR.Server/Controllers/Accounting/AccountingReportsController.cs`
- `AestheticEMR/AestheticEMR.Server/Services/Reporting/ILegacyCrystalReportProxyService.cs`
- `AestheticEMR/AestheticEMR.Server/Services/Reporting/LegacyCrystalReportProxyService.cs`
- `CrystalReportWebAPI/CrystalReportWebAPI/Controllers/ReportsController.cs`
- `CrystalReportWebAPI/CrystalReportWebAPI/Utilities/DapperReportData.cs`
- `AestheticEMR/AestheticEMR.client/src/app/features/reports/accounting/accounting-reports.component.ts`

---

## References

- VB.NET Balance Sheet Form: `C:\Users\Administrator\source\repos\Accounting\Accounting\frmRptBalSheet.vb`
- VB.NET GL Form: `C:\Users\Administrator\source\repos\Accounting\Accounting\frmRptGL.vb`
- Legacy Tran Class: `C:\Users\Administrator\source\repos\Accounting\Accounting\Classes\Tran.vb`

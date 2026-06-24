using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class BalanceSheetHeaders3
{
    public string CoyID { get; set; } = null!;

    public string Period { get; set; } = null!;

    public string? PeriodVal { get; set; }

    public string? RptType { get; set; }

    public decimal? Accounts_Payable { get; set; }

    public decimal? Accounts_Receivable { get; set; }

    public decimal? Accrued_Income_Tax { get; set; }

    public decimal? Accumulated_Depreciation { get; set; }

    public decimal? Bank_Accounts { get; set; }

    public decimal? Bank_OD_Accounts { get; set; }

    public decimal? Capital_Account { get; set; }

    public decimal? Cash { get; set; }

    public decimal? Deposits { get; set; }

    public decimal? Duties___Trade_Taxes { get; set; }

    public decimal? Equity { get; set; }

    public decimal? Inventory { get; set; }

    public decimal? Land___Building { get; set; }

    public decimal? Machinery { get; set; }

    public decimal? Other_Current_Assets { get; set; }

    public decimal? Other_Fixed_Assets { get; set; }

    public decimal? Retained_Earnings { get; set; }

    public decimal? Secured_Loans { get; set; }

    public decimal? Shares { get; set; }

    public decimal? Unsecured_Loans { get; set; }

    public decimal? Vehicles { get; set; }

    public decimal? All_SUM { get; set; }
}

using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwBalanceSheetHeaders2
{
    public string Period { get; set; } = null!;

    public string CoyID { get; set; } = null!;

    public decimal? Cash { get; set; }

    public decimal? Bank_Accounts { get; set; }

    public decimal? AccountsReceivable { get; set; }

    public decimal? OtherCurrentAssets { get; set; }

    public decimal? Inventory { get; set; }

    public decimal? LandAndBuilding { get; set; }

    public decimal? Machinery { get; set; }

    public decimal? Vehicles { get; set; }

    public decimal? OtherFixedAssets { get; set; }

    public decimal? AccumulatedDepreciation { get; set; }

    public decimal? SecuredLoans { get; set; }

    public decimal? UnsecuredLoans { get; set; }

    public decimal? AccountsPayable { get; set; }

    public decimal? DutiesAndTradeTaxes { get; set; }

    public decimal? BankODAccounts { get; set; }

    public decimal? AccruedIncomeTax { get; set; }

    public decimal? Equity { get; set; }

    public decimal? CapitalAccount { get; set; }

    public decimal? RetainedEarnings { get; set; }

    public decimal? Shares { get; set; }

    public int? Yr { get; set; }

    public int? MonthCounter { get; set; }

    public string? PeriodVal { get; set; }

    public decimal? Total { get; set; }
}

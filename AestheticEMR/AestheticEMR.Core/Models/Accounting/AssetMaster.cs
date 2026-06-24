using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class AssetMaster
{
    public long SNo { get; set; }

    public string? AssetCode { get; set; }

    public string AssetName { get; set; } = null!;

    public string? AccountNo { get; set; }

    public string? AcctNoAccumDepr { get; set; }

    public decimal AssetValue { get; set; }

    public decimal? ScrapValue { get; set; }

    public decimal UserLife { get; set; }

    public decimal? DepreciationRate { get; set; }

    public decimal? TotalDepreciation { get; set; }

    public decimal? NetBookValue { get; set; }

    public decimal? DurationInMths { get; set; }

    public int? DeprCount { get; set; }

    public DateTime DepStartDate { get; set; }

    public DateTime? DepEndDate { get; set; }

    public string GroupCode { get; set; } = null!;

    public string SubGroupCode { get; set; } = null!;

    public string LocationCode { get; set; } = null!;

    public string DepartmentCode { get; set; } = null!;

    public DateTime PurchaseDate { get; set; }

    public string InvoiceNumber { get; set; } = null!;

    public string Supplier { get; set; } = null!;

    public string InsuranceCompany { get; set; } = null!;

    public string InsuranceNo { get; set; } = null!;

    public string PolicyType { get; set; } = null!;

    public decimal PremiumValue { get; set; }

    public decimal InsuranceAmount { get; set; }

    public string? DisposedYN { get; set; }

    public DateTime? DisposedDate { get; set; }

    public bool? DeprActive { get; set; }

    public byte[]? Img { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public string? CoyID { get; set; }

    public string? AssetDesc { get; set; }
}

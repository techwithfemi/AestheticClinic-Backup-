using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class DepreciationMaster
{
    public long Sno { get; set; }

    public DateTime EntryDate { get; set; }

    public DateTime AquireDate { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? DisposalDate { get; set; }

    public DateTime? DateLastDepr { get; set; }

    public int DurationInMths { get; set; }

    public decimal GrossValue { get; set; }

    public decimal? DeprAmount { get; set; }

    public decimal? AccumDeprAmount { get; set; }

    public decimal? SalvageValue { get; set; }

    public int? DeprCount { get; set; }

    public string? AccountId { get; set; }
}

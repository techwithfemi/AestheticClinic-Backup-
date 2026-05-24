using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HInvestigateFinding
{
    public long Id { get; set; }

    public DateTime InvDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string? SympItem { get; set; }

    public string? Result { get; set; }

    public string? Remarks { get; set; }

    public string? Clientcat { get; set; }

    public bool? AttendedTo { get; set; }

    public string? EmpId { get; set; }

    public string? ConId { get; set; }

    public string? SympItemCat { get; set; }

    public string? Capitated { get; set; }

    public bool? AttendedTobyLab { get; set; }

    public bool? Suppres { get; set; }

    public double? Price { get; set; }

    public double? SubTotal { get; set; }

    public double? Qty { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public bool? IsPost { get; set; }

    public bool? SuppresForAcct { get; set; }

    public string? TranId { get; set; }

    public decimal? Cost { get; set; }

    public bool? Reversed { get; set; }

    public long? ReversedPair { get; set; }

    public long? LabItemSno { get; set; }
}

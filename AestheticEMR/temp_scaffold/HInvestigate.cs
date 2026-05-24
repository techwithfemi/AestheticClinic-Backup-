using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HInvestigate
{
    public long Id { get; set; }

    public DateTime InvDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string? Investigate { get; set; }

    public string? InvResult { get; set; }

    public string ClientCat { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string? ConId { get; set; }

    public bool? AttendedTobyLab { get; set; }

    public string? SympItemCat { get; set; }

    public bool? Suppres { get; set; }

    public string? Capitated { get; set; }

    public string? LabNum { get; set; }

    public DateTime? TimeVal { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public DateTime? EntryDate2 { get; set; }

    public DateTime? EntryTime2 { get; set; }
}

using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwUserActivity
{
    public long Sno { get; set; }

    public string? Fullname { get; set; }

    public string Username { get; set; } = null!;

    public DateTime LoginDate { get; set; }

    public DateTime LoginTime { get; set; }

    public bool? IsLogOut { get; set; }

    public DateTime? LogOutDate { get; set; }

    public DateTime? LogOutTime { get; set; }

    public string? Remarks { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public int? Appversion { get; set; }

    public string EmpId { get; set; } = null!;
}

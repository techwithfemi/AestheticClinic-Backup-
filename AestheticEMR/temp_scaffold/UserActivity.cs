using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class UserActivity
{
    public long Sno { get; set; }

    public string Username { get; set; } = null!;

    public DateTime LoginDate { get; set; }

    public DateTime LoginTime { get; set; }

    public bool? IsLogOut { get; set; }

    public DateTime? LogOutDate { get; set; }

    public DateTime? LogOutTime { get; set; }

    public bool? AutoLogoff { get; set; }

    public string? Remarks { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public int? AppVersion { get; set; }
}

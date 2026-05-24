using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class LogInTray
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? FullName { get; set; }

    public DateTime? LogInTime { get; set; }

    public DateTime? LogOutTime { get; set; }

    public DateTime? Date { get; set; }

    public string? RemoteMachine { get; set; }
}

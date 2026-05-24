using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class LoginTrail
{
    public string? SerialNo { get; set; }

    public string? Username { get; set; }

    public string? LoginType { get; set; }

    public DateTime? LogDate { get; set; }

    public DateTime? LogTime { get; set; }
}

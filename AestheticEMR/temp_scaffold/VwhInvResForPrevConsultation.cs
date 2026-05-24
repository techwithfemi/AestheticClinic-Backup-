using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhInvResForPrevConsultation
{
    public long Id { get; set; }

    public string PNo { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public DateTime InvDate { get; set; }

    public string? Investigate { get; set; }

    public string? InvResultX { get; set; }

    public string InvResult { get; set; } = null!;

    public string Labno { get; set; } = null!;

    public string TreatedBy { get; set; } = null!;

    public string? Range { get; set; }

    public string? Unit { get; set; }

    public string? Description { get; set; }

    public long InvResId { get; set; }
}

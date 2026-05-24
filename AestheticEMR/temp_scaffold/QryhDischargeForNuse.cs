using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhDischargeForNuse
{
    public long Id { get; set; }

    public DateTime DischDate { get; set; }

    public DateTime DischTime { get; set; }

    public string Pno { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public string WardId { get; set; } = null!;

    public string? Reason { get; set; }

    public string? Remarks { get; set; }

    public DateTime? ApptDate { get; set; }

    public string Fullname { get; set; } = null!;

    public string StaffName { get; set; } = null!;

    public string StaffNo { get; set; } = null!;

    public string Expr1 { get; set; } = null!;
}

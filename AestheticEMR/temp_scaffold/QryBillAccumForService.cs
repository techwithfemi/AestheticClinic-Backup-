using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryBillAccumForService
{
    public long Sno { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime Date { get; set; }

    public string Service { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public decimal Qty { get; set; }

    public decimal? SubTotal { get; set; }

    public string PatNo { get; set; } = null!;

    public string? Fullname { get; set; }

    public string Billtype { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string CatRemarks { get; set; } = null!;

    public string? PCatId { get; set; }

    public string? CoyName { get; set; }

    public string PSurname { get; set; } = null!;

    public string? PFirstname { get; set; }

    public string? ConId { get; set; }

    public string RetainId { get; set; } = null!;

    public string RetainName { get; set; } = null!;
}

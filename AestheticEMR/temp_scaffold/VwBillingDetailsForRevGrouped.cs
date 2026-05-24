using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingDetailsForRevGrouped
{
    public DateTime Date { get; set; }

    public string Fullname { get; set; } = null!;

    public string Company { get; set; } = null!;

    public string? PCatId { get; set; }

    public string? RevType { get; set; }

    public string BillNo { get; set; } = null!;

    public decimal? SubTotal { get; set; }

    public string? ClientCatId { get; set; }

    public string RetainId { get; set; } = null!;

    public string? RetainCode { get; set; }

    public string BillBy { get; set; } = null!;

    public string? ClientType { get; set; }
}

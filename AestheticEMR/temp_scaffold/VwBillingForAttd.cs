using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingForAttd
{
    public string BillNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string Company { get; set; } = null!;

    public DateTime Date { get; set; }

    public string? ClientCatId { get; set; }
}

using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwInvoiceForPatient
{
    public string BillNo { get; set; } = null!;

    public double? STotal { get; set; }

    public string PNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string Diagnosis { get; set; } = null!;

    public string? CoyName { get; set; }

    public string? PCatId { get; set; }

    public string? CatRemarks { get; set; }
}

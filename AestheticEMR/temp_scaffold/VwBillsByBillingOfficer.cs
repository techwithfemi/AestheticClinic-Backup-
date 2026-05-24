using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillsByBillingOfficer
{
    public DateTime BillDate { get; set; }

    public DateTime RecDate { get; set; }

    public string PNo { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string DrgName { get; set; } = null!;

    public double SubTotal { get; set; }

    public string? EmpName { get; set; }

    public string? EmpId { get; set; }

    public string Company { get; set; } = null!;
}

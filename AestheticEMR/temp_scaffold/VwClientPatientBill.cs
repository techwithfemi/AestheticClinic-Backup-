using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwClientPatientBill
{
    public DateTime RecDate { get; set; }

    public DateTime DtDate { get; set; }

    public string Fullname { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public string DrgName { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public double SubTotal { get; set; }

    public DateTime? AdmDate { get; set; }

    public DateTime? DischDate { get; set; }

    public int? NumDays { get; set; }

    public string Diagnosis { get; set; } = null!;

    public string? Dosage { get; set; }

    public int? Age { get; set; }

    public string? EmpNo { get; set; }

    public string? CoyName { get; set; }

    public string? CoyCode { get; set; }

    public string? BatchNo { get; set; }

    public string? Company { get; set; }

    public string? BillingMonth { get; set; }

    public int? BillingYear { get; set; }
}

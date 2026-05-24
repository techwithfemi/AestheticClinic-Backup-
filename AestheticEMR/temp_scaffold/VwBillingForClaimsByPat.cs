using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingForClaimsByPat
{
    public int Sno { get; set; }

    public string Fullname { get; set; } = null!;

    public string? CatRemarks { get; set; }

    public string BilltRemarks { get; set; } = null!;

    public string Service { get; set; } = null!;

    public string DrgName { get; set; } = null!;

    public double Price { get; set; }

    public double? Qty { get; set; }

    public decimal? SubTotal { get; set; }

    public DateTime BDate { get; set; }

    public DateTime ConsultDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string? OldpNo { get; set; }

    public string? Diagnosis { get; set; }

    public decimal? AmountBilled { get; set; }

    public string? AmountBilledInWord { get; set; }

    public decimal AmountPaid { get; set; }

    public string CoyName { get; set; } = null!;

    public string? PCatId { get; set; }

    public string? DrgcatGroup { get; set; }

    public string? Age { get; set; }

    public string? EmpNo { get; set; }

    public DateTime? DischDate { get; set; }

    public DateTime? AdmDate { get; set; }

    public string? ApprvCode { get; set; }

    public string? Sex { get; set; }

    public string? PPhoneNo { get; set; }

    public string? Email { get; set; }

    public string? PolicyType { get; set; }

    public string Company { get; set; } = null!;

    public DateTime Date { get; set; }

    public string? Referal { get; set; }

    public string BillTo { get; set; } = null!;

    public string BillToNo { get; set; } = null!;

    public string? Dosage { get; set; }

    public string? RevType { get; set; }

    public decimal Discount { get; set; }

    public decimal Debt { get; set; }

    public string ClinicType { get; set; } = null!;

    public string? BillType { get; set; }
}

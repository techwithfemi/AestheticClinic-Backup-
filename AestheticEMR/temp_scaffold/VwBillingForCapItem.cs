using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingForCapItem
{
    public DateTime Date { get; set; }

    public DateTime BDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string? OldpNo { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Diagnosis { get; set; }

    public int AmountBilled { get; set; }

    public string AmountBilledInWord { get; set; } = null!;

    public int AmountPaid { get; set; }

    public string? CoyName { get; set; }

    public string? PCatId { get; set; }

    public string PNo { get; set; } = null!;

    public int? Age { get; set; }

    public string? EmpNo { get; set; }

    public DateTime? DischDate { get; set; }

    public DateTime? AdmDate { get; set; }

    public string ApprvCode { get; set; } = null!;

    public string? Sex { get; set; }

    public string? PPhoneNo { get; set; }

    public string? Email { get; set; }

    public string? PolicyType { get; set; }

    public string? Company { get; set; }

    public string InvNo { get; set; } = null!;

    public DateTime DtDate { get; set; }

    public string DrgName { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public double SubTotal { get; set; }

    public string DrgCatGroup { get; set; } = null!;

    public string CatRemarks { get; set; } = null!;

    public string Service { get; set; } = null!;

    public DateTime ConsultDate { get; set; }

    public string? Referal { get; set; }

    public bool? IsBilled { get; set; }

    public bool? AttendedTo { get; set; }

    public int Sno { get; set; }

    public string Billtype { get; set; } = null!;
}

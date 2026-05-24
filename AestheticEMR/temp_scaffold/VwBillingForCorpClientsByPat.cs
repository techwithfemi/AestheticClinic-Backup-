using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingForCorpClientsByPat
{
    public int Sno { get; set; }

    public string Fullname { get; set; } = null!;

    public string BilltRemarks { get; set; } = null!;

    public double? SubTotal { get; set; }

    public DateTime BDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string? OldpNo { get; set; }

    public string Diagnosis { get; set; } = null!;

    public string? CoyName { get; set; }

    public string? Age { get; set; }

    public DateTime? DischDate { get; set; }

    public DateTime? AdmDate { get; set; }

    public string? Sex { get; set; }

    public string? PPhoneNo { get; set; }

    public string? Email { get; set; }

    public string Company { get; set; } = null!;

    public string? BillTo { get; set; }

    public string? BillToNo { get; set; }

    public string ClinicType { get; set; } = null!;

    public string? PCatId { get; set; }

    public decimal AmountPaid { get; set; }

    public decimal AmountBilled { get; set; }
}

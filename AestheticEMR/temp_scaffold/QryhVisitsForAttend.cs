using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhVisitsForAttend
{
    public DateTime RecDate { get; set; }

    public DateTime? Time { get; set; }

    public string? OldPno { get; set; }

    public string FullName { get; set; } = null!;

    public string ClinicType { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public string? EmpNo { get; set; }

    public string? PhoneNo { get; set; }

    public string? Area { get; set; }

    public string? Username { get; set; }

    public string? ClientCat { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string? Referal { get; set; }

    public string Status { get; set; } = null!;

    public string RetainCode { get; set; } = null!;

    public string RetainName { get; set; } = null!;

    public string RetainId { get; set; } = null!;

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }

    public string? Remarks { get; set; }

    public decimal? AmountBal { get; set; }

    public decimal? AmountCap { get; set; }

    public DateTime? BillDate { get; set; }

    public string? PcatId { get; set; }

    public string? PPhoneno { get; set; }

    public int? Age { get; set; }

    public DateTime? Dob { get; set; }

    public string? Email { get; set; }

    public string? Hmoref { get; set; }
}

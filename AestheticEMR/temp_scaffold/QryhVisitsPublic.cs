using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhVisitsPublic
{
    public DateTime RecDate { get; set; }

    public DateTime? Time { get; set; }

    public string PNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string ClinicType { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime? NextApptDate { get; set; }

    public int RecId { get; set; }

    public string? ClientCat { get; set; }

    public string ClientCatId { get; set; } = null!;

    public string? EmpId { get; set; }

    public string ConsultId { get; set; } = null!;

    public string OldpNo { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string PolicyType { get; set; } = null!;

    public string CoyType { get; set; } = null!;

    public string PCatId { get; set; } = null!;

    public string? Referal { get; set; }

    public string Expr1 { get; set; } = null!;

    public string EmpNo { get; set; } = null!;

    public string Branch { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Area { get; set; } = null!;

    public string LatestBillNo { get; set; } = null!;

    public string? PPhoneNo { get; set; }

    public string UserName { get; set; } = null!;

    public string BioId { get; set; } = null!;

    public string ClientCatId2 { get; set; } = null!;

    public DateTime Date1 { get; set; }

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }

    public string RetainId { get; set; } = null!;

    public string RetainCode { get; set; } = null!;

    public string? Mth { get; set; }

    public string? Yr { get; set; }

    public DateTime? EntryDate { get; set; }

    public string RetainName { get; set; } = null!;

    public decimal? AmountBal { get; set; }

    public decimal? AmountCap { get; set; }

    public DateTime BillDate { get; set; }

    public string Email { get; set; } = null!;

    public int? Age { get; set; }
}

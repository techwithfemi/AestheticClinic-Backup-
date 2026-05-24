using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhvisitsForSearch
{
    public DateTime RecDate { get; set; }

    public DateTime? Time { get; set; }

    public string PNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string ClinicType { get; set; } = null!;

    public string? PolicyType { get; set; }

    public string? Remarks { get; set; }

    public DateTime? NextApptDate { get; set; }

    public int RecId { get; set; }

    public string? ClientCat { get; set; }

    public string? EmpId { get; set; }

    public string ConsultId { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string CoyName { get; set; } = null!;

    public string? CoyType { get; set; }

    public string? EmpNo { get; set; }

    public string? Branch { get; set; }

    public string? Status { get; set; }

    public string? Referal { get; set; }

    public string Surname { get; set; } = null!;

    public string RetainName { get; set; } = null!;

    public bool? AttendedToByNurse { get; set; }

    public DateOnly? ExitDate { get; set; }

    public bool? AttendedToByDoc { get; set; }

    public string RetainId { get; set; } = null!;

    public string RetainCode { get; set; } = null!;

    public string? HmoRef { get; set; }

    public string? DocAssigned { get; set; }

    public string? Doctor { get; set; }

    public byte? PatVal { get; set; }
}

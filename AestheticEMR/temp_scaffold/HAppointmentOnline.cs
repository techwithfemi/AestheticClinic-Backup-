using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HAppointmentOnline
{
    public long Id { get; set; }

    public string Pno { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public DateTime? ApptDate { get; set; }

    public DateTime? ApptTime { get; set; }

    public string? ClinicType { get; set; }

    public string? Remarks { get; set; }

    public bool? AttendedTo { get; set; }

    public string? ConId { get; set; }

    public bool? Suppres { get; set; }

    public string? RetainCode { get; set; }

    public string? Fullname { get; set; }

    public string? Phone { get; set; }

    public string? EnrolleEmail { get; set; }

    public string? EmpName { get; set; }

    public string? EmpPhone { get; set; }

    public string? EmpEmail { get; set; }

    public string? ApprvCode { get; set; }
}

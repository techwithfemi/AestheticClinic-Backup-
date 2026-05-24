using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HRecords241019
{
    public int RecId { get; set; }

    public DateTime RecDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string? ClientCat { get; set; }

    public string? Remarks { get; set; }

    public string? EmpId { get; set; }

    public string ClinicType { get; set; } = null!;

    public DateTime? NextApptDate { get; set; }

    public DateTime? Htime { get; set; }

    public bool? AttendedTo { get; set; }

    public string? Referal { get; set; }

    public string? DocAssigned { get; set; }

    public bool? AttendedToByDoc { get; set; }

    public byte? PatVal { get; set; }

    public bool? Suppres { get; set; }

    public string? Mth { get; set; }

    public string? Yr { get; set; }

    public DateOnly? ExitDate { get; set; }

    public string? ExitDateComment { get; set; }

    public string? Diagnosis { get; set; }

    public string? Coyname { get; set; }

    public bool? AttendedToByNurse { get; set; }

    public DateTime? BillDate { get; set; }

    public string? ConsultIdnew { get; set; }

    public string? ConsultIdnew2 { get; set; }

    public bool? IsJson { get; set; }

    public string? AttndStatus { get; set; }

    public string? Tariff { get; set; }

    public decimal? Debt { get; set; }

    public bool? AttendedToByImmume { get; set; }

    public string? HmoRef { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public string? LastConsultId { get; set; }

    public DateTime? LastAttndDate { get; set; }

    public string? LastClinicVisited { get; set; }

    public string? LastPurpose { get; set; }
}

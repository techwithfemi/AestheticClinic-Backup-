using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class HRecord
{
    public int RecId { get; set; } //identity

    public DateTime RecDate { get; set; }

    public string ConsultId { get; set; } = null!; //primary key

    public string PNo { get; set; } = null!; //foreign key to patient table 

    public string? ClientCat { get; set; } // clientCatID from patient table

    public string? Remarks { get; set; }

    public string? EmpId { get; set; } // empid of username that created the record

    public string ClinicType { get; set; } = null!;

    public DateTime? NextApptDate { get; set; }

    public DateTime? Htime { get; set; }  // time of record creation

    public bool? AttendedTo { get; set; }=false;

    public string? Referal { get; set; }= "NO";

    public string? DocAssigned { get; set; } // empid of doctor assigned to patient for this record

    public bool? AttendedToByDoc { get; set; }

    public byte? PatVal { get; set; }

    public bool? Suppres { get; set; }= false;

    public string? Mth { get; set; } // padded month of record creation eg 03

    public string? Yr { get; set; } // padded year of record creation eg 2024

    public DateOnly? ExitDate { get; set; }

    public string? ExitDateComment { get; set; }

    public string? Diagnosis { get; set; }

    public string? Coyname { get; set; } // retainID from company table

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

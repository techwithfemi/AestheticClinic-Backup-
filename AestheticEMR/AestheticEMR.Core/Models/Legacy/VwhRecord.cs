using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class VwhRecord
{
    public int RecId { get; set; } //identity from hrecords

    public DateTime RecDate { get; set; } // attendance date from hrecords

    public string ConsultId { get; set; } = null!; // consultid from hrecords

    public string PNo { get; set; } = null!;

    public string? ClientCat { get; set; }

    public string? Remarks { get; set; } // same as purpose in front end

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

    public DateTime? ExitDate { get; set; }

    public string? ExitDateComment { get; set; }

    public string? Mth { get; set; }

    public string? Yr { get; set; }

    public string? BatchVal { get; set; }

    public string? BatchNo { get; set; }

    public string? Diagnosis { get; set; }

    public string? Coyname { get; set; } //same as coyID in front end

    public DateTime? BillDate { get; set; }

    public string? RetainCode { get; set; } //same as coyID in front end

    public string RetainName { get; set; } = null!; //same as client/company name in front end

    public string? ClientCatId { get; set; } // same as ClientType in hretainership

    public string? ClientType { get; set; }

    public string Fullname { get; set; } = null!;

    public string? AcctId { get; set; }

    public DateTime? Dob { get; set; }

    public string? Sex { get; set; }

    public int? Age { get; set; }

    public string? MonthName { get; set; }

    public string? RetainId { get; set; } //same as coyID in front end

    public decimal? RegAmount { get; set; }

    public decimal? ConAmount { get; set; }

    public decimal? CardRenewAmount { get; set; }

    public string? CoyType { get; set; }

    public string? PCatId { get; set; }

    public string? ClientCatId2 { get; set; }

    public string? OldpNo { get; set; }

    public string? PhoneNo { get; set; }

    public double? Debt { get; set; }

    public string? PolicyType { get; set; }

    public string? EmpNo { get; set; }
}

namespace AestheticEMR.Server.ViewModels.Legacy;

public class VwhRecordSummaryVM
{
    public int? RecId { get; set; }
    public DateTime? RecDate { get; set; }
    public string ConsultId { get; set; } = string.Empty;
    public string PNo { get; set; } = string.Empty;
    public string? ClientCat { get; set; }
    public string? Remarks { get; set; }
    public string? EmpId { get; set; }
    public string ClinicType { get; set; } = string.Empty;
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
    public string? Diagnosis { get; set; }
    public string? Coyname { get; set; }
    public DateTime? BillDate { get; set; }
    public string? RetainCode { get; set; }
    public string RetainName { get; set; } = string.Empty;
    public string? ClientCatId { get; set; }
    public string? ClientType { get; set; }
    public string Fullname { get; set; } = string.Empty;
    public string? AcctId { get; set; }
    public DateTime? Dob { get; set; }
    public string? Sex { get; set; }
    public int? Age { get; set; }
    public string? MonthName { get; set; }
    public string? RetainId { get; set; }
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
    public string? PatientPhotoBase64 { get; set; }
}

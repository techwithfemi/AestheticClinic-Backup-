using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class HPatient
{
    public string Pno { get; set; } = null!;

    public string? OldPno { get; set; }

    public string PSurName { get; set; } = null!;

    public string? PFirstname { get; set; }

    public string? Title { get; set; }

    public string? Sex { get; set; }

    public string? HomeAddress { get; set; }

    public DateTime? Dob { get; set; }

    public string? Occupation { get; set; }

    public string? OfficeAddress { get; set; }

    public string? Religion { get; set; }

    public string? NextofKin { get; set; }

    public string? KinAddress { get; set; }

    public string? RelationToKin { get; set; }

    public string? PPhoneNo { get; set; }

    public string? Mstatus { get; set; }

    public string? BloodGroup { get; set; }

    public string? Genotype { get; set; }

    public string? PCatId { get; set; }

    public string? CoyType { get; set; }

    public string? CoyName { get; set; }

    public string? ClientCatId { get; set; }

    public DateTime? RegDate { get; set; }

    public string? FileDuration { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? Email { get; set; }

    public string? Ref { get; set; }

    public string? EmpNo { get; set; }

    public string? Branch { get; set; }

    public string? Status { get; set; }

    public string? RelationToStaff { get; set; }

    public string? Introducedby { get; set; }

    public string? PolicyType { get; set; }

    public string? CardType { get; set; }

    public string? PMembers { get; set; }

    public bool? Expired { get; set; }

    public string? Maturity { get; set; }

    public decimal? Debt { get; set; }

    public decimal? DebtBf { get; set; }

    public string? Color { get; set; }

    public string? DrgRxn { get; set; }

    public string? CoyClass { get; set; }

    public string? Nokphone { get; set; }

    public string? HmoRef { get; set; }

    public string? Principal { get; set; }

    public string? PastMedHist { get; set; }

    public string? Area { get; set; }

    public byte[]? PatPix { get; set; }

    public string? LatestBillNo { get; set; }

    public DateTime? LastAttndDate { get; set; }

    public string? LastConsultId { get; set; }

    public string? UserName { get; set; }

    public bool? IsEnrol { get; set; }

    public string? BioId { get; set; }

    public string? Ancinfo { get; set; }

    public string? Pno2 { get; set; }

    public string? NewReg { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? SmsNextDob { get; set; }

    public string? SmsCat { get; set; }

    public bool? IsRev { get; set; }

    public long Sno { get; set; }

    public string? LastClinicVisited { get; set; }

    public string? LastPurpose { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public string? LastDoctorSeen { get; set; }

    public DateTime? LasConDate { get; set; }

    public DateTime? LastConDate { get; set; }

    public int? AdmissionDaysLimit { get; set; }

    public int? CumNoOfAdmissionDaysPerAnnum { get; set; }

    public DateTime? TranStartDateForDebt { get; set; }

    public DateTime? LastCheckDateForDebt { get; set; }

    public string? Faculty { get; set; }

    public string? Session { get; set; }

    public string? PixName { get; set; }

    public string? Course { get; set; }
}

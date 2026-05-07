using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class Vwhpatient
{
    public string Surname { get; set; } = null!;

    public string? Firstname { get; set; }

    public string Client { get; set; } = null!;

    public DateTime? LastAttndDate { get; set; }

    public decimal? Debt { get; set; }

    public string? LastClinicVisited { get; set; }

    public string? Purpose { get; set; }

    public string? CardNo { get; set; }

    public string Pno { get; set; } = null!;

    public string? PatCat { get; set; }

    public string? CoyClass { get; set; }

    public string? CoyType { get; set; }

    public string? CoyName { get; set; }

    public string? BillingCat { get; set; }

    public DateTime? RegDate { get; set; }

    public string? FileDuration { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? HomeAddress { get; set; }

    public string? OfficeAddress { get; set; }

    public string? PhoneNo { get; set; }

    public DateTime? Dob { get; set; }

    public string? Email { get; set; }

    public string? Sex { get; set; }

    public string? Ref { get; set; }

    public string? OldPno { get; set; }

    public string? EmpNo { get; set; }

    public string? Branch { get; set; }

    public string? Status { get; set; }

    public string? RelationToStaff { get; set; }

    public string Introducedby { get; set; } = null!;

    public string? PolicyType { get; set; }

    public string? CardType { get; set; }

    public string? FamMembers { get; set; }

    public string? BloodGroup { get; set; }

    public string? Genotype { get; set; }

    public string? Occupation { get; set; }

    public string? Religion { get; set; }

    public string? NextofKin { get; set; }

    public string? RelationToKin { get; set; }

    public string? KinAddress { get; set; }

    public bool? Expired { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Marital { get; set; }

    public string? Nokphone { get; set; }

    public string? Title { get; set; }

    public string? HmoRef { get; set; }

    public string? Mstatus { get; set; }

    public string? Maturity { get; set; }

    public string? PastMedHist { get; set; }

    public string? Area { get; set; }

    public string? Principal { get; set; }

    public string? LatestBillNo { get; set; }

    public string? LastConsultId { get; set; }

    public string? UserName { get; set; }

    public string? NewReg { get; set; }

    public string RetainCode { get; set; } = null!;

    public DateTime? EntryDate { get; set; }

    public string? LastDoctorSeen { get; set; }

    public DateTime? LastConDate { get; set; }

    public int? AdmissionDaysLimit { get; set; }

    public int? CumNoOfAdmissionDaysPerAnnum { get; set; }
}

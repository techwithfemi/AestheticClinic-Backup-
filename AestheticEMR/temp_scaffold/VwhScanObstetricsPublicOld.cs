using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhScanObstetricsPublicOld
{
    public long Sno { get; set; }

    public long ConId { get; set; }

    public string? EmpName { get; set; }

    public string? AgeVal { get; set; }

    public DateTime? InvDate { get; set; }

    public string? LabNo { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Pno { get; set; }

    public string? Uterus { get; set; }

    public string? NofOfGest { get; set; }

    public string? Presentation { get; set; }

    public string? LieAsOfToday { get; set; }

    public string? FoetalWb { get; set; }

    public string? FoetalCp { get; set; }

    public string? FoetalLm { get; set; }

    public string? PlacentaLocate { get; set; }

    public string? PlacentaMg { get; set; }

    public string? PlacentaPt { get; set; }

    public string? InternalCos { get; set; }

    public string? AmnioticFc { get; set; }

    public string? GrossFad { get; set; }

    public string? AdnexalPd { get; set; }

    public string? OtherUterineMd { get; set; }

    public string? FoetalPwt { get; set; }

    public string? LikelyGender { get; set; }

    public string? Bpd { get; set; }

    public string? Fl { get; set; }

    public string? Ac { get; set; }

    public string? Edd { get; set; }

    public string? Diagnosis { get; set; }

    public string? Comments { get; set; }

    public string? DocName { get; set; }

    public string? HospName { get; set; }
}

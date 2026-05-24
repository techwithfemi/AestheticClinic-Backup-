using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HDialysi
{
    public long Sno { get; set; }

    public string ConsultId { get; set; } = null!;

    public string DialNo { get; set; } = null!;

    public string? HaemoRec { get; set; }

    public DateTime DiaDate { get; set; }

    public string Diagnosis { get; set; } = null!;

    public DateTime? ConnectionTime { get; set; }

    public DateTime? DisconnectionTime { get; set; }

    public string? PreDialAssess { get; set; }

    public string? PostDialAssess { get; set; }

    public string? Hiv { get; set; }

    public string? HbsAg { get; set; }

    public string? Pcv { get; set; }

    public string? BloodGp { get; set; }

    public DateTime? ClottingTime { get; set; }

    public string? DialPresc { get; set; }

    public string? MachineType { get; set; }

    public string? DialyserType { get; set; }

    public string? New { get; set; }

    public string? Second { get; set; }

    public string? Third { get; set; }

    public string? Fourth { get; set; }

    public string? ConcType { get; set; }

    public string? Duration { get; set; }

    public string? AccessRoute { get; set; }

    public string? ReqdWtLoss { get; set; }

    public string? Tmp { get; set; }

    public string? Ufr { get; set; }

    public string? Bfr { get; set; }

    public string? Heparin { get; set; }

    public string? InfusionsDrug { get; set; }

    public string? BloodTransfusion { get; set; }

    public string? ConnectedBy { get; set; }

    public decimal? CurrentWt { get; set; }

    public decimal? DryWt { get; set; }

    public decimal? PrevPostDialWt { get; set; }

    public decimal? WtGain { get; set; }

    public decimal? PostDialWt { get; set; }

    public decimal? WtLoss { get; set; }
}

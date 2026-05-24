using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HSurgery
{
    public int Id { get; set; }

    public DateTime SDate { get; set; }

    public string PNo { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string? Indications { get; set; }

    public string? Operation { get; set; }

    public string? Consent { get; set; }

    public string? Relation { get; set; }

    public string? Urinalysis { get; set; }

    public string? Sediments { get; set; }

    public string? Sugar { get; set; }

    public string? Acetone { get; set; }

    public string? Aib { get; set; }

    public string? Sg { get; set; }

    public string? Blood { get; set; }

    public string? Hb { get; set; }

    public string? Pcv { get; set; }

    public string? Wbc { get; set; }

    public string? WbcP { get; set; }

    public string? WbcL { get; set; }

    public string? WbcM { get; set; }

    public string? WbcE { get; set; }

    public string? WbcEsr { get; set; }

    public string? Urea { get; set; }

    public string? UreaNa { get; set; }

    public string? UreaCl { get; set; }

    public string? UreaPco3 { get; set; }

    public string? OccultBlood { get; set; }

    public string? ChestXray { get; set; }

    public string? Ecg { get; set; }

    public string? BloodGroup { get; set; }

    public string? Surgeon { get; set; }

    public string? Assistant { get; set; }

    public string? Anaesthetist { get; set; }

    public string? PreOpbp { get; set; }

    public string? Pulse { get; set; }

    public string? PostOpbp { get; set; }

    public string? HgP { get; set; }

    public string? Findings { get; set; }

    public string? Prosedure { get; set; }

    public string? Hiv { get; set; }

    public string? EmpId { get; set; }

    public long? ConId { get; set; }

    public string? AnaesthNotePre { get; set; }

    public string? AnaesthNotePost { get; set; }
}

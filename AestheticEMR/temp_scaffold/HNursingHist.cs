using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HNursingHist
{
    public DateTime? DtDate { get; set; }

    public string? PNo { get; set; }

    public string? ConsultId { get; set; }

    public string? EmpId { get; set; }

    public string? Informnt { get; set; }

    public string? Present { get; set; }

    public string? Past { get; set; }

    public string? Nutrition { get; set; }

    public string? Elimin { get; set; }

    public string? Exercise { get; set; }

    public string? Sleep { get; set; }

    public string? Comm { get; set; }

    public string? Perception { get; set; }

    public string? SocialStat { get; set; }

    public string? Sexuality { get; set; }

    public string? Stress { get; set; }

    public string? Beliefs { get; set; }

    public string? Habits { get; set; }

    public string? Valuables { get; set; }

    public string? Urinalysis { get; set; }

    public string? GenInspec { get; set; }

    public string? Palpation { get; set; }

    public string? Percussion { get; set; }

    public string? Auscultation { get; set; }

    public string? LabResult { get; set; }

    public string? NurseDiag { get; set; }

    public string? Allergies { get; set; }

    public bool? Bcg { get; set; }

    public bool? Polio { get; set; }

    public bool? Tetanus { get; set; }

    public bool? Whooping { get; set; }

    public bool? Dipthe { get; set; }

    public bool? Measles { get; set; }

    public string? Others { get; set; }

    public DateTime? Lmp { get; set; }

    public DateTime? Edd { get; set; }

    public string? NurAssess { get; set; }

    public long Id { get; set; }

    public string? Objectives { get; set; }

    public string? NurOrders { get; set; }

    public string? Evaluation { get; set; }
}

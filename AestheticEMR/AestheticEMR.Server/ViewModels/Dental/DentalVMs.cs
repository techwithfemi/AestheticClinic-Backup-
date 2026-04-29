using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Dental;

/// <summary>Maps to HDentalTreat — the real odontogram / dental treatment record.</summary>
public class DentalChartVM
{
    public long Id { get; set; }

    [Required]
    [StringLength(20)]
    public required string Pno { get; set; }

    [Required]
    [StringLength(20)]
    public required string ConsultId { get; set; }

    [StringLength(100)]
    public string? Dtype { get; set; }

    [Required]
    public DateTime TDate { get; set; }

    public DateTime TTime { get; set; }

    // Adult Upper Left quadrant
    public bool? Auli1 { get; set; }
    public bool? Auli2 { get; set; }
    public bool? Aulc { get; set; }
    public bool? Aulpm1 { get; set; }
    public bool? Aulpm2 { get; set; }
    public bool? Aulm1 { get; set; }
    public bool? Aulm2 { get; set; }
    public bool? Aulm3 { get; set; }

    // Adult Upper Right quadrant
    public bool? Auri1 { get; set; }
    public bool? Auri2 { get; set; }
    public bool? Aurc { get; set; }
    public bool? Aurpm1 { get; set; }
    public bool? Aurpm2 { get; set; }
    public bool? Aurm1 { get; set; }
    public bool? Aurm2 { get; set; }
    public bool? Aurm3 { get; set; }

    // Adult Lower Left quadrant
    public bool? Alli1 { get; set; }
    public bool? Alli2 { get; set; }
    public bool? Allc { get; set; }
    public bool? Allpm1 { get; set; }
    public bool? Allpm2 { get; set; }
    public bool? Allm1 { get; set; }
    public bool? Allm2 { get; set; }
    public bool? Allm3 { get; set; }

    // Adult Lower Right quadrant
    public bool? Alri1 { get; set; }
    public bool? Alri2 { get; set; }
    public bool? Alrc { get; set; }
    public bool? Alrpm1 { get; set; }
    public bool? Alrpm2 { get; set; }
    public bool? Alrm1 { get; set; }
    public bool? Alrm2 { get; set; }
    public bool? Alrm3 { get; set; }

    // Child Upper Left quadrant
    public bool? Culi1 { get; set; }
    public bool? Culi2 { get; set; }
    public bool? Culc { get; set; }
    public bool? Culpm1 { get; set; }
    public bool? Culpm2 { get; set; }

    // Child Upper Right quadrant
    public bool? Curi1 { get; set; }
    public bool? Curi2 { get; set; }
    public bool? Curc { get; set; }
    public bool? Curpm1 { get; set; }
    public bool? Curpm2 { get; set; }

    // Child Lower Left quadrant
    public bool? Clli1 { get; set; }
    public bool? Clli2 { get; set; }
    public bool? Cllc { get; set; }
    public bool? Cllpm1 { get; set; }
    public bool? Cllpm2 { get; set; }

    // Child Lower Right quadrant
    public bool? Clri1 { get; set; }
    public bool? Clri2 { get; set; }
    public bool? Clrc { get; set; }
    public bool? Clrpm1 { get; set; }
    public bool? Clrpm2 { get; set; }

    public string? ARem { get; set; }   // Adult remarks
    public string? CRem { get; set; }   // Child remarks
    public string? ConId { get; set; }

    public string? PatientName { get; set; }
}

public class DentalImagingVM
{
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public required string Pno { get; set; }

    [Required]
    [StringLength(20)]
    public required string ConsultId { get; set; }

    [Required]
    public DateTime ImagingDate { get; set; }

    [StringLength(100)]
    public string? ImagingType { get; set; }

    [StringLength(200)]
    public string? ToothRegion { get; set; }

    public string? Findings { get; set; }
    public string? Impression { get; set; }
    public string? Recommendations { get; set; }
    public string? FilePath { get; set; }

    [StringLength(255)]
    public string? FileName { get; set; }

    public string? Notes { get; set; }
    public string? PatientName { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
}

public class DentalConsultingVM
{
    public long Id { get; set; }

    [Required]
    [StringLength(20)]
    public required string ConsultId { get; set; }

    [Required]
    [StringLength(20)]
    public required string PNo { get; set; }

    [Required]
    [StringLength(50)]
    public required string ClientCat { get; set; }

    public string? Diagnosis { get; set; }
    public string? Prescription { get; set; }
    public string? Services { get; set; }
    public string? Investigate { get; set; }

    public string? TreatPlan { get; set; }
}

public class DentalEncounterSaveVM
{
    [Required]
    public required DentalChartVM Chart { get; set; }

    [Required]
    public required DentalImagingVM Imaging { get; set; }

    [Required]
    public required DentalConsultingVM Consulting { get; set; }
}

public class DentalEncounterVM
{
    public required DentalChartVM Chart { get; set; }
    public required DentalImagingVM Imaging { get; set; }
    public required DentalConsultingVM Consulting { get; set; }
}

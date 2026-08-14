using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhConsultingPatientsForBilling
{
    [StringLength(50)]
    public string pNO { get; set; } = null!;

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime preDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? preTime { get; set; }

    public int recID { get; set; }

    public bool? suppres { get; set; }

    [StringLength(50)]
    public string clinicType { get; set; } = null!;

    [StringLength(50)]
    public string pSurname { get; set; } = null!;

    [StringLength(50)]
    public string pFirstname { get; set; } = null!;

    [StringLength(50)]
    public string? Company { get; set; }

    [StringLength(100)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string? clientCat { get; set; }

    public int? Age { get; set; }

    [StringLength(50)]
    public string Sex { get; set; } = null!;

    [StringLength(50)]
    public string? occupation { get; set; }

    [StringLength(50)]
    public string? bloodGroup { get; set; }

    [StringLength(50)]
    public string? genotype { get; set; }

    [StringLength(50)]
    public string? status { get; set; }
}

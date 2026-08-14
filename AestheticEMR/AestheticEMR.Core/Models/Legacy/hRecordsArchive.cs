using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hRecordsArchive")]
public partial class hRecordsArchive
{
    public int recID { get; set; }

    public DateOnly recDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(100)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    [StringLength(50)]
    public string clinicType { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? NextApptDate { get; set; }

    public DateOnly? htime { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? referal { get; set; }

    [StringLength(50)]
    public string? DocAssigned { get; set; }

    public bool? attendedToByDoc { get; set; }

    public byte? PatVal { get; set; }

    public bool? suppres { get; set; }

    [StringLength(5)]
    public string? Mth { get; set; }

    [StringLength(5)]
    public string? Yr { get; set; }

    public DateOnly? ExitDate { get; set; }

    [StringLength(50)]
    public string? ExitComment { get; set; }

    [StringLength(1050)]
    public string? Diagnosis { get; set; }
}

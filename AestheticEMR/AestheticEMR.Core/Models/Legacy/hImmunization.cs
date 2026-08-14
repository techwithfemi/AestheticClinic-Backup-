using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hImmunization")]
public partial class hImmunization
{
    public long ID { get; set; }

    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(50)]
    public string ClientCat { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime ImDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ImTime { get; set; }

    [StringLength(50)]
    public string AgeValue { get; set; } = null!;

    [StringLength(100)]
    public string Immunization { get; set; } = null!;

    [StringLength(50)]
    public string EmpID { get; set; } = null!;

    [StringLength(100)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NextApptDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NextApptTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ImmType { get; set; }
}

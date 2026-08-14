using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hNurseAllergy")]
public partial class hNurseAllergy
{
    public long SNo { get; set; }

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime ADate { get; set; }

    [StringLength(500)]
    public string Event { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime tReaction { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime tDoc { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string Symptoms { get; set; } = null!;

    [StringLength(500)]
    public string? Others { get; set; }

    [StringLength(500)]
    public string? Comments { get; set; }

    [StringLength(508)]
    [Unicode(false)]
    public string Note { get; set; } = null!;

    [StringLength(50)]
    public string Nurse { get; set; } = null!;

    [StringLength(50)]
    public string Doctor { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhNurseAllergy
{
    public long SNo { get; set; }

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(500)]
    public string Event { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime ReactionTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TimeSeenByDoctor { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string SignsAndSymptoms { get; set; } = null!;

    [StringLength(500)]
    public string? Others { get; set; }

    [StringLength(500)]
    public string? Comments { get; set; }

    [StringLength(508)]
    [Unicode(false)]
    public string Note { get; set; } = null!;

    [StringLength(101)]
    public string? Nurse { get; set; }

    [StringLength(101)]
    public string? Doctor { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string NurseID { get; set; } = null!;

    [StringLength(50)]
    public string DocID { get; set; } = null!;
}

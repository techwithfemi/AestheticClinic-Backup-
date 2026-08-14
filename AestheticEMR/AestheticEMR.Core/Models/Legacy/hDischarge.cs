using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("hDischarge")]
public partial class hDischarge
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dischDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dischTime { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string pno { get; set; } = null!;

    [Key]
    [StringLength(50)]
    [Unicode(false)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string ClientCat { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? WardID { get; set; }

    [StringLength(5550)]
    [Unicode(false)]
    public string? Reason { get; set; }

    [StringLength(5550)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApptDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ApprvBy { get; set; }

    [Unicode(false)]
    public string? Recommend { get; set; }

    [Unicode(false)]
    public string? ResponseToDrug { get; set; }

    [Unicode(false)]
    public string? DrugsGiven { get; set; }

    [Unicode(false)]
    public string? DiagnosisFindings { get; set; }

    [Unicode(false)]
    public string? ChiefCompaints { get; set; }
}

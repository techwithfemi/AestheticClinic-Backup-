using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hRecordsArchiveOLD")]
public partial class hRecordsArchiveOLD
{
    public int recID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime recDate { get; set; }

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

    [Column(TypeName = "datetime")]
    public DateTime? htime { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? referal { get; set; }
}

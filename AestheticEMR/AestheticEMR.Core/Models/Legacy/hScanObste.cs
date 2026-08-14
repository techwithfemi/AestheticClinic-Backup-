using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hScanObste")]
public partial class hScanObste
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime INVDATE { get; set; }

    [StringLength(50)]
    public string PNO { get; set; } = null!;

    [StringLength(50)]
    public string LABNO { get; set; } = null!;

    [StringLength(250)]
    public string Description { get; set; } = null!;

    [StringLength(50)]
    public string? desc2 { get; set; }

    [StringLength(4000)]
    public string Result { get; set; } = null!;

    [StringLength(2500)]
    public string InvResult { get; set; } = null!;

    [StringLength(50)]
    public string EMPID { get; set; } = null!;

    public bool? ATTENDEDTO { get; set; }

    public long ConID { get; set; }

    [StringLength(1000)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string? Class { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? ImageID { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? CRL { get; set; }
}

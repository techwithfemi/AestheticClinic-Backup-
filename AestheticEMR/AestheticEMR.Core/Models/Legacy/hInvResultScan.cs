using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hInvResultScan")]
public partial class hInvResultScan
{
    public long ID { get; set; }

    [StringLength(250)]
    public string PNO { get; set; } = null!;

    [StringLength(50)]
    public string LABNO { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime INVDATE { get; set; }

    [StringLength(4000)]
    public string RESULT { get; set; } = null!;

    [StringLength(4000)]
    public string REMARKS { get; set; } = null!;

    [StringLength(50)]
    public string EMPID { get; set; } = null!;

    public bool? ATTENDEDTO { get; set; }

    public long? conID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Dept { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryBillAccumCashList
{
    public int SNo { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(50)]
    public string PatNo { get; set; } = null!;

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    public bool? suppres { get; set; }

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(3000)]
    [Unicode(false)]
    public string Remarks { get; set; } = null!;

    [StringLength(50)]
    public string? clientCatID { get; set; }

    [StringLength(50)]
    public string clinicType { get; set; } = null!;
}

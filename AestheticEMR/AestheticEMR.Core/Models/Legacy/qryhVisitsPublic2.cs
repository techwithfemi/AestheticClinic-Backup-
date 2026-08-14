using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhVisitsPublic2
{
    [Column(TypeName = "datetime")]
    public DateTime recDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Time { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(61)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string clinicType { get; set; } = null!;

    [StringLength(100)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NextApptDate { get; set; }

    public int recID { get; set; }

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(7)]
    [Unicode(false)]
    public string coyNAme { get; set; } = null!;

    public bool? attendedTo { get; set; }

    [StringLength(7)]
    [Unicode(false)]
    public string pCatID { get; set; } = null!;
}

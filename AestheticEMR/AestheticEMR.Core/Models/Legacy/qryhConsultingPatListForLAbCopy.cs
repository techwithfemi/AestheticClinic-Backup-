using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhConsultingPatListForLAbCopy
{
    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pno { get; set; } = null!;

    [StringLength(2000)]
    public string? investigate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime invDate { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string pSurname { get; set; } = null!;

    [StringLength(50)]
    public string pFirstname { get; set; } = null!;

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    [StringLength(200)]
    [Unicode(false)]
    public string? remarks { get; set; }

    [StringLength(101)]
    public string treatedby { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(154)]
    public string? company { get; set; }

    [StringLength(50)]
    public string? coyNAme { get; set; }
}

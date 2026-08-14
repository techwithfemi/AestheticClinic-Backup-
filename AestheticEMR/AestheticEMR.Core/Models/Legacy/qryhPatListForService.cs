using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhPatListForService
{
    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime recDate { get; set; }

    [StringLength(50)]
    public string pSurname { get; set; } = null!;

    [StringLength(50)]
    public string pFirstname { get; set; } = null!;

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(100)]
    public string? Remarks { get; set; }

    [StringLength(101)]
    public string treatedby { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(154)]
    public string? company { get; set; }

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;
}

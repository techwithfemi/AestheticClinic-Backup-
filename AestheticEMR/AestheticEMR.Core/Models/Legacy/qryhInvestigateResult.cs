using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhInvestigateResult
{
    [Column(TypeName = "datetime")]
    public DateTime invDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string pno { get; set; } = null!;

    [StringLength(2000)]
    public string? investigate { get; set; }

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    [StringLength(2000)]
    public string? sympItem { get; set; }

    [StringLength(2000)]
    public string? result { get; set; }

    [StringLength(400)]
    public string? remarks { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(101)]
    public string? Empname { get; set; }

    [StringLength(2000)]
    public string? invResult { get; set; }
}

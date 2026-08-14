using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhInvestigateResultPublic
{
    [Column(TypeName = "datetime")]
    public DateTime invDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string pno { get; set; } = null!;

    public string? investigate { get; set; }

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    public string? sympItem { get; set; }

    [StringLength(400)]
    public string? remarks { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(116)]
    public string? Empname { get; set; }

    public string? Result { get; set; }
}

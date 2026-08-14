using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhConsultingVisitsForCoy
{
    public DateOnly Date { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(254)]
    public string? company { get; set; }

    [StringLength(150)]
    public string CoyName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? referal { get; set; }

    [StringLength(150)]
    public string retainName { get; set; } = null!;

    [StringLength(30)]
    public string? Ref { get; set; }

    [StringLength(50)]
    public string retainID { get; set; } = null!;

    public double? debt { get; set; }

    [StringLength(150)]
    public string pSurname { get; set; } = null!;

    [StringLength(150)]
    public string? pFirstname { get; set; }

    [StringLength(50)]
    public string? clientCatID { get; set; }

    public bool? isBilled { get; set; }

    [StringLength(50)]
    public string Remarks { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhConsultingVisit
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(251)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string? ClientCat { get; set; }

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(50)]
    public string PNo { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(354)]
    public string? company { get; set; }

    [StringLength(150)]
    public string CoyName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? referal { get; set; }

    [StringLength(150)]
    public string RetainName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Ref { get; set; }

    [StringLength(50)]
    public string RetainID { get; set; } = null!;

    public double? debt { get; set; }

    [StringLength(100)]
    public string pSurname { get; set; } = null!;

    [StringLength(150)]
    public string? pFirstname { get; set; }

    [StringLength(50)]
    public string? clientCatID { get; set; }

    public bool? isBilled { get; set; }

    [StringLength(50)]
    public string Remarks { get; set; } = null!;

    [StringLength(3)]
    [Unicode(false)]
    public string Status { get; set; } = null!;
}

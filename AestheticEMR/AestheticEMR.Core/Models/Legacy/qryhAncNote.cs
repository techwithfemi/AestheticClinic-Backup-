using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhAncNote
{
    public int SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dtDate { get; set; }

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? pNo { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? genHealth { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string? ht { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? heart { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? lungs { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Teeth { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Veins { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Nipples { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Para { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ClinicPelvimetry { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? conjugate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? transverse { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Intertub { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PostSaggital { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? xRayNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? EDDScan { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? empID { get; set; }
}

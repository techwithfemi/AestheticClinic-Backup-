using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("SensitivityOLD")]
public partial class SensitivityOLD
{
    public long ID { get; set; }

    [StringLength(50)]
    public string LABNO { get; set; } = null!;

    [StringLength(50)]
    public string? AMPICILLIN { get; set; }

    [StringLength(50)]
    public string? CLOXACILLIN { get; set; }

    [StringLength(50)]
    public string? ERYTHRO { get; set; }

    [StringLength(50)]
    public string? TETRACYCL { get; set; }

    [StringLength(50)]
    public string? COTRIMO { get; set; }

    [StringLength(50)]
    public string? NITROF { get; set; }

    [StringLength(50)]
    public string? TARIVID { get; set; }

    [StringLength(50)]
    public string? NALIDIX { get; set; }

    [StringLength(50)]
    public string? CIPROFL { get; set; }

    [StringLength(50)]
    public string? CEFOT { get; set; }

    [StringLength(50)]
    public string? CHLORAMPH { get; set; }

    [StringLength(50)]
    public string? GENTAMYCIN { get; set; }

    [StringLength(50)]
    public string? FORTUM { get; set; }

    [StringLength(50)]
    public string? AUGMENTIN { get; set; }

    [StringLength(50)]
    public string? ROCPHINE { get; set; }

    [StringLength(50)]
    public string? CLINDAMYC { get; set; }

    [StringLength(50)]
    public string? PEFLOX { get; set; }

    [StringLength(50)]
    public string? LEVOFLOX { get; set; }

    [StringLength(500)]
    public string? REMARKS { get; set; }
}

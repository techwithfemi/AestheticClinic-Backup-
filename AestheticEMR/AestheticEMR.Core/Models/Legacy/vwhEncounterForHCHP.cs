using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhEncounterForHCHP
{
    [StringLength(50)]
    public string? empNo { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(100)]
    public string? Visittype { get; set; }

    public DateOnly? EncDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dischDate { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? ConDForDisch { get; set; }

    [StringLength(116)]
    public string Doctor { get; set; } = null!;

    [StringLength(500)]
    public string? Diag { get; set; }

    [StringLength(500)]
    public string? OtherDiag { get; set; }

    [StringLength(500)]
    public string? Lab { get; set; }

    [StringLength(500)]
    public string? OtherLab { get; set; }

    [StringLength(500)]
    public string? Drug { get; set; }

    [StringLength(500)]
    public string? OtherDrug { get; set; }

    [StringLength(500)]
    public string? Procd { get; set; }

    [StringLength(500)]
    public string? OtherProcd { get; set; }

    [StringLength(500)]
    public string? Remarks { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string pSurname { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string? pFirstname { get; set; }
}

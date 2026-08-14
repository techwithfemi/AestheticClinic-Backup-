using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hEncounterForHCHP")]
public partial class hEncounterForHCHP
{
    public long SNo { get; set; }

    public long? ConID { get; set; }

    [StringLength(50)]
    public string? ConsultID { get; set; }

    [StringLength(50)]
    public string? PNo { get; set; }

    public DateOnly? EncDate { get; set; }

    [StringLength(50)]
    public string? DischCond { get; set; }

    [StringLength(50)]
    public string? EncType { get; set; }

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
}

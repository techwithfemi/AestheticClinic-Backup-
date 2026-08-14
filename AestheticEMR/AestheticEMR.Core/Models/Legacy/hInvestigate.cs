using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hInvestigate")]
public partial class hInvestigate
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime invDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pno { get; set; } = null!;

    public string? investigate { get; set; }

    public string? invResult { get; set; }

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    public bool? attendedTobyLab { get; set; }

    [StringLength(500)]
    public string? sympItemCat { get; set; }

    public bool? suppres { get; set; }

    [StringLength(3)]
    public string? Capitated { get; set; }

    [StringLength(50)]
    public string? LabNum { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? timeVal { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate2 { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime2 { get; set; }
}

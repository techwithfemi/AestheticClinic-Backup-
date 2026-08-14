using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[PrimaryKey("drgName", "drgCatName", "remarks")]
[Table("LabServicesOLD2")]
public partial class LabServicesOLD2
{
    [Key]
    [StringLength(250)]
    public string drgName { get; set; } = null!;

    [Key]
    [StringLength(50)]
    public string drgCatName { get; set; } = null!;

    [StringLength(50)]
    public string? qtyPerUnit { get; set; }

    public double Cost { get; set; }

    public double? NHISCost { get; set; }

    public double? price { get; set; }

    [Key]
    [StringLength(50)]
    [Unicode(false)]
    public string remarks { get; set; } = null!;
}

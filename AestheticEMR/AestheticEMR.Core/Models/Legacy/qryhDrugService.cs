using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhDrugService
{
    [StringLength(250)]
    public string drgName { get; set; } = null!;

    [StringLength(50)]
    public string drgCatName { get; set; } = null!;

    [StringLength(50)]
    public string? drgCatGroup { get; set; }

    [StringLength(50)]
    public string? qtyPerUnit { get; set; }

    public double Cost { get; set; }

    public double? NHISCost { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? remarks { get; set; }
}

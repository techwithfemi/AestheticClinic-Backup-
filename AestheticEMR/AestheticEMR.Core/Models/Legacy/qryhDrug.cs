using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhDrug
{
    [StringLength(250)]
    public string drgName { get; set; } = null!;

    [StringLength(50)]
    public string drgCatName { get; set; } = null!;

    [StringLength(50)]
    public string? qtyPerUnit { get; set; }

    public double Cost { get; set; }

    public double? price { get; set; }

    [StringLength(150)]
    public string? catRemarks { get; set; }

    public double? NHISCost { get; set; }

    [StringLength(50)]
    public string? deptBillCenter { get; set; }
}

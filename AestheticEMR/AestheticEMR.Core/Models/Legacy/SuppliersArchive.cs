using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("SuppliersArchive")]
public partial class SuppliersArchive
{
    public long ID { get; set; }

    [StringLength(50)]
    public string SupplierID { get; set; } = null!;

    [StringLength(50)]
    public string SupplierName { get; set; } = null!;

    [StringLength(50)]
    public string? ContactName { get; set; }

    [StringLength(30)]
    public string? ContactTitle { get; set; }

    [StringLength(60)]
    public string? Address { get; set; }

    [StringLength(24)]
    public string? Phone { get; set; }

    [StringLength(50)]
    public string? email { get; set; }

    [StringLength(50)]
    public string? Category { get; set; }

    public double? Credit { get; set; }
}

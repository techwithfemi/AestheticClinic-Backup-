using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("PurchaseOrderGen")]
public partial class PurchaseOrderGen
{
    [StringLength(50)]
    public string POID { get; set; } = null!;

    public long? SupplierID { get; set; }

    [StringLength(50)]
    public string? EmpID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpectedDate { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? Suppres { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("PurchaseOrderApproveGen")]
public partial class PurchaseOrderApproveGen
{
    [StringLength(50)]
    public string POID { get; set; } = null!;

    [StringLength(50)]
    public string? EmpID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    [StringLength(500)]
    public string? Remarks { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? Suppres { get; set; }

    public long ID { get; set; }
}

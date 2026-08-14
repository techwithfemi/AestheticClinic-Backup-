using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryCustomerPendingStock
{
    [Column(TypeName = "datetime")]
    public DateTime EntryDate { get; set; }

    [StringLength(50)]
    public string ItemID { get; set; } = null!;

    [StringLength(50)]
    public string ItemName { get; set; } = null!;

    public int Qty { get; set; }

    [StringLength(50)]
    public string EnteredBy { get; set; } = null!;

    [StringLength(50)]
    public string? CustID { get; set; }

    [StringLength(200)]
    public string? Comments { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpectedDate { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hRetainershipArchive")]
public partial class hRetainershipArchive
{
    [Column(TypeName = "smalldatetime")]
    public DateTime retainDate { get; set; }

    [StringLength(50)]
    public string RetainCode { get; set; } = null!;

    [StringLength(50)]
    public string retainID { get; set; } = null!;

    [StringLength(150)]
    public string retainName { get; set; } = null!;

    [StringLength(50)]
    public string? clientCatID { get; set; }

    [StringLength(50)]
    public string Address { get; set; } = null!;

    [StringLength(50)]
    public string? PhoneNo { get; set; }

    [StringLength(50)]
    public string? email { get; set; }

    [StringLength(50)]
    public string? Contact { get; set; }

    public double? ProfFee { get; set; }

    public double? Debt { get; set; }

    public int? BillEndDate { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwInvBatchNo2
{
    [Column(TypeName = "datetime")]
    public DateTime? InvDate { get; set; }

    [StringLength(50)]
    public string? BatchNo { get; set; }

    [StringLength(7)]
    public string? BatchNo2 { get; set; }

    [StringLength(50)]
    public string CoyCode { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string RetainName { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryBillingBalanceDueForPrivateGroupedOLD
{
    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    public string? clientID { get; set; }

    [Column(TypeName = "decimal(38, 0)")]
    public decimal? totDue { get; set; }

    [StringLength(50)]
    public string pCatID { get; set; } = null!;
}

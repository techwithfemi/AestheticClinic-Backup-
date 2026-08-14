using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryBillingForCLientsGroupedByMonthNotYetPaid
{
    [StringLength(50)]
    public string BillingMonth { get; set; } = null!;

    public int BillingYear { get; set; }

    [StringLength(50)]
    public string clientID { get; set; } = null!;

    [StringLength(150)]
    public string CLIENTName { get; set; } = null!;

    [Column(TypeName = "numeric(38, 0)")]
    public decimal? Amount { get; set; }
}

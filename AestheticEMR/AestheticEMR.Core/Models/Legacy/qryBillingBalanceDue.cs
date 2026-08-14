using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryBillingBalanceDue
{
    [Column(TypeName = "smalldatetime")]
    public DateTime bDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? consultDate { get; set; }

    [StringLength(50)]
    public string clientID { get; set; } = null!;

    [StringLength(150)]
    public string CLIENTName { get; set; } = null!;

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [Column(TypeName = "numeric(18, 0)")]
    public decimal AmountBilled { get; set; }

    [Column(TypeName = "numeric(18, 0)")]
    public decimal AmountPaid { get; set; }

    [Column(TypeName = "numeric(19, 0)")]
    public decimal? AmountDue { get; set; }
}

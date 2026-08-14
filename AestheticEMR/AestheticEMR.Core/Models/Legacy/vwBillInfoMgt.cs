using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillInfoMgt
{
    [StringLength(50)]
    public string BillNo { get; set; } = null!;

    public double AmountGen { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountPaid { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(150)]
    public string Company { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime AttdDate { get; set; }

    [StringLength(1050)]
    public string? Diagnosis { get; set; }

    [StringLength(100)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string? clientCatID { get; set; }
}

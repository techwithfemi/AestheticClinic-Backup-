using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingDetailsForSalesCat
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(355)]
    public string Fullname { get; set; } = null!;

    [StringLength(150)]
    public string Company { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? pCatID { get; set; }

    [StringLength(50)]
    public string? RevType { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? subTotal { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? clientCatID { get; set; }

    [StringLength(50)]
    public string retainID { get; set; } = null!;

    [StringLength(50)]
    public string? RetainCode { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string BillBy { get; set; } = null!;

    [StringLength(50)]
    public string? ClientType { get; set; }

    [StringLength(6)]
    [Unicode(false)]
    public string SalesCat { get; set; } = null!;
}

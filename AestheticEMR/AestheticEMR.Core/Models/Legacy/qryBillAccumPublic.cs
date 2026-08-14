using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryBillAccumPublic
{
    public int SNo { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(550)]
    public string Service { get; set; } = null!;

    public double UnitPrice { get; set; }

    public double Qty { get; set; }

    public double subTotal { get; set; }

    [StringLength(50)]
    public string PatNo { get; set; } = null!;

    [StringLength(61)]
    public string Fullname { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string billtype { get; set; } = null!;

    public bool? attendedTo { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string catRemarks { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string retainID { get; set; } = null!;

    [StringLength(7)]
    [Unicode(false)]
    public string pCatID { get; set; } = null!;

    [StringLength(7)]
    [Unicode(false)]
    public string coyNAme { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string pSurname { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string pFirstname { get; set; } = null!;

    [StringLength(50)]
    public string? conID { get; set; }

    [StringLength(100)]
    public string? Remarks { get; set; }
}

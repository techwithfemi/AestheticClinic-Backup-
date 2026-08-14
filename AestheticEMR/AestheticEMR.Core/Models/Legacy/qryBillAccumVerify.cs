using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryBillAccumVerify
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

    [StringLength(150)]
    [Unicode(false)]
    public string billtype { get; set; } = null!;

    public bool? attendedTo { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string catRemarks { get; set; } = null!;

    [StringLength(50)]
    public string? conID { get; set; }

    public bool? suppres { get; set; }

    [StringLength(3)]
    public string? Capitated { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? referal { get; set; }

    public bool? isBilled { get; set; }

    [StringLength(500)]
    public string? Dosage { get; set; }

    public double ProfFee { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(50)]
    public string? pCatID { get; set; }

    [StringLength(150)]
    public string pSurname { get; set; } = null!;

    [StringLength(150)]
    public string? pFirstname { get; set; }

    [StringLength(50)]
    public string? clientCatID { get; set; }

    [StringLength(50)]
    public string? retainID { get; set; }

    [StringLength(30)]
    public string? Ref { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BillTo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CoyName { get; set; }
}

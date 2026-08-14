using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhVisitsPublic
{
    [Column(TypeName = "datetime")]
    public DateTime recDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Time { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string clinicType { get; set; } = null!;

    [StringLength(100)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NextApptDate { get; set; }

    public int recID { get; set; }

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(7)]
    [Unicode(false)]
    public string ClientCatID { get; set; } = null!;

    [StringLength(50)]
    public string? empID { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string oldpNo { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string coyNAme { get; set; } = null!;

    public bool? attendedTo { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string policyType { get; set; } = null!;

    [StringLength(7)]
    [Unicode(false)]
    public string CoyType { get; set; } = null!;

    [StringLength(7)]
    [Unicode(false)]
    public string pCatID { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? referal { get; set; }

    [StringLength(7)]
    [Unicode(false)]
    public string Expr1 { get; set; } = null!;

    [StringLength(50)]
    public string empNo { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string branch { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string status { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string Area { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string LatestBillNo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? pPhoneNo { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string UserName { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string BioID { get; set; } = null!;

    [StringLength(7)]
    [Unicode(false)]
    public string clientCatID2 { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime date1 { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountPaid { get; set; }

    [StringLength(50)]
    public string retainID { get; set; } = null!;

    [StringLength(50)]
    public string RetainCode { get; set; } = null!;

    [StringLength(2)]
    public string? Mth { get; set; }

    [StringLength(30)]
    public string? Yr { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string RetainName { get; set; } = null!;

    [Column(TypeName = "decimal(20, 2)")]
    public decimal? AmountBal { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? AmountCap { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime BillDAte { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string email { get; set; } = null!;

    public int? Age { get; set; }
}

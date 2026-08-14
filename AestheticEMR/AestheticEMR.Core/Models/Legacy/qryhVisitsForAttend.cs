using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhVisitsForAttend
{
    [Column(TypeName = "datetime")]
    public DateTime RecDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Time { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? OLdPNo { get; set; }

    [StringLength(1001)]
    public string FullName { get; set; } = null!;

    [StringLength(50)]
    public string ClinicType { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string CoyName { get; set; } = null!;

    [StringLength(100)]
    public string? EmpNo { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PhoneNo { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Area { get; set; }

    [StringLength(101)]
    public string? Username { get; set; }

    [StringLength(50)]
    public string? ClientCat { get; set; }

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? referal { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [StringLength(50)]
    public string retainCode { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string RetainName { get; set; } = null!;

    [StringLength(50)]
    public string RetainID { get; set; } = null!;

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountPaid { get; set; }

    [StringLength(100)]
    public string? Remarks { get; set; }

    [Column(TypeName = "decimal(20, 2)")]
    public decimal? AmountBal { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? AmountCap { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BillDate { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PCatID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? pPhoneno { get; set; }

    public int? Age { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOB { get; set; }

    [StringLength(500)]
    public string? Email { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? HMORef { get; set; }
}

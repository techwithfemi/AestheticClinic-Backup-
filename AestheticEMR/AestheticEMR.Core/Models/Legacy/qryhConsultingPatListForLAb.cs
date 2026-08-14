using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhConsultingPatListForLAb
{
    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(3000)]
    [Unicode(false)]
    public string? investigate { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string pSurname { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? pFirstname { get; set; }

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string Remarks { get; set; } = null!;

    [StringLength(101)]
    public string? treatedby { get; set; }

    public int? Age { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string CoyCode { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Coyname { get; set; }

    [StringLength(50)]
    public string? ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? cDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? cTime { get; set; }

    public bool? suppres { get; set; }

    public bool? AttendedtoByLab { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Ref { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string retainID { get; set; } = null!;

    [StringLength(50)]
    public string RetainCode { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime invDate { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? policyType { get; set; }

    [StringLength(100)]
    public string? empNo { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? symptoms { get; set; }

    [Unicode(false)]
    public string? diagnosis { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? pPhoneNo { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? maturity { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PhoneNo { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [StringLength(50)]
    public string? LabNum { get; set; }
}

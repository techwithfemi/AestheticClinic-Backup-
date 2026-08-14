using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("ClinicTypes_260325")]
public partial class ClinicTypes_260325
{
    public long SNO { get; set; }

    [StringLength(50)]
    public string ClinicID { get; set; } = null!;

    [StringLength(100)]
    public string ClinicName { get; set; } = null!;

    [StringLength(3)]
    public string? Type { get; set; }

    [StringLength(250)]
    public string? clinicDays { get; set; }

    public double? RegFee { get; set; }

    public double? ConFee { get; set; }

    [StringLength(50)]
    public string? Code { get; set; }

    public bool? IsVitals { get; set; }

    [StringLength(2)]
    public string? RctCode { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Designation { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? EmpID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PixName { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? IDValCode { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PhoneNo { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClinicPeriod { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Apologies { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwHImmunization
{
    public long ID { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime ImDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ImTime { get; set; }

    [StringLength(50)]
    public string AgeValue { get; set; } = null!;

    [StringLength(100)]
    public string Immunization { get; set; } = null!;

    [StringLength(50)]
    public string EmpID { get; set; } = null!;

    [StringLength(100)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(101)]
    public string? EmpName { get; set; }

    [StringLength(101)]
    public string? StaffName { get; set; }
}

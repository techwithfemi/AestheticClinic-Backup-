using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDocClinicAndPatAssigned
{
    [StringLength(406)]
    public string Patient { get; set; } = null!;

    [StringLength(65)]
    public string? Doctor { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PhoneNo { get; set; }

    [StringLength(50)]
    public string EmpID { get; set; } = null!;

    [StringLength(50)]
    public string clinicID { get; set; } = null!;

    [StringLength(100)]
    public string ClinicName { get; set; } = null!;

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    public byte? PatVal { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    public bool? attendedToByDoc { get; set; }
}

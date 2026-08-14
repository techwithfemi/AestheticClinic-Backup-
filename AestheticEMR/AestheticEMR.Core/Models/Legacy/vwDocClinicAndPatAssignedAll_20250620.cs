using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDocClinicAndPatAssignedAll_20250620
{
    [StringLength(1001)]
    public string Patient { get; set; } = null!;

    [StringLength(1001)]
    public string Doctor { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? PhoneNo { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    [StringLength(50)]
    public string clinicID { get; set; } = null!;

    [StringLength(50)]
    public string ClinicName { get; set; } = null!;

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    public byte? PatVal { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    public bool? attendedToByDoc { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string RoomNo { get; set; } = null!;

    public int IsOld { get; set; }
}

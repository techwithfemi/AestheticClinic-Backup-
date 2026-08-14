using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhNextAppt
{
    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    public string? oldpNo { get; set; }

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? apptDate { get; set; }

    [StringLength(50)]
    public string referTo { get; set; } = null!;

    [StringLength(1000)]
    public string refReason { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? refDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? refTime { get; set; }

    public bool? AttendedTo { get; set; }
}

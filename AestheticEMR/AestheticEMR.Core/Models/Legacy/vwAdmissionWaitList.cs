using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwAdmissionWaitList
{
    [Column(TypeName = "datetime")]
    public DateTime? Date { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Time { get; set; }

    [StringLength(50)]
    public string? RoomNo { get; set; }

    [StringLength(65)]
    public string? Doctor { get; set; }

    public byte NumOfPat { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(500)]
    public string? referTo { get; set; }

    public bool? AttendedTo { get; set; }

    [StringLength(1000)]
    public string? refReason { get; set; }

    [StringLength(301)]
    public string Patient { get; set; } = null!;

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOB { get; set; }

    public int? Age { get; set; }
}

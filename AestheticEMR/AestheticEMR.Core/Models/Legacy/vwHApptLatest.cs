using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwHApptLatest
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? entryTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApptDate { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? ApptTime { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? ApptTime2 { get; set; }

    [StringLength(406)]
    public string Patient { get; set; } = null!;

    [StringLength(101)]
    public string? GivenBy { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    public int? Age { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Company { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string PNo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string? conID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PhoneNo { get; set; }

    public bool? AttendedToByRec { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? clinicType { get; set; }
}

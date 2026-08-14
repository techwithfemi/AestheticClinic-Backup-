using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhAppt
{
    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    public string? oldpNo { get; set; }

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    public long ID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string clientCat { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime entryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime entryTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ApptDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ApptTime { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string clinicType { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? remarks { get; set; }

    public bool? attendedTo { get; set; }

    public bool? suppres { get; set; }
}

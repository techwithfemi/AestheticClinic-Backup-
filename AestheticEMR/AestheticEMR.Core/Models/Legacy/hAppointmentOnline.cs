using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hAppointmentOnline")]
public partial class hAppointmentOnline
{
    public long ID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string pno { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string clientCat { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? entryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? entryTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApptDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApptTime { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? clinicType { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? remarks { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    public bool? suppres { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? RetainCode { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? Fullname { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? EnrolleEmail { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? EmpName { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? EmpPhone { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? EmpEmail { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? ApprvCode { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhReferal
{
    public long ID { get; set; }

    [StringLength(50)]
    public string? pNo { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? apptDate { get; set; }

    [StringLength(500)]
    public string? referTo { get; set; }

    [StringLength(1000)]
    public string? refReason { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? refDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? refTime { get; set; }

    public bool? AttendedTo { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string pSurname { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? pFirstname { get; set; }

    [StringLength(2000)]
    public string? refAddress { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Coyname { get; set; }

    public bool? suppres { get; set; }

    [StringLength(50)]
    public string? treatedBy { get; set; }

    [StringLength(101)]
    public string? ReferedBy { get; set; }

    public bool? AttendedToByRec { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [StringLength(3000)]
    [Unicode(false)]
    public string? diagnosis { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? diffDiagnosis { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }
}

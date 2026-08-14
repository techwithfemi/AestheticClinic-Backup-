using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhReferalForRpt
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    public long ID { get; set; }

    [StringLength(50)]
    public string? pNo { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string? clientCat { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? apptDate { get; set; }

    [StringLength(500)]
    public string? ClinicType { get; set; }

    [StringLength(1000)]
    public string? refReason { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? refDate { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? Time { get; set; }

    [StringLength(2000)]
    public string? refAddress { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CoyID { get; set; }

    public bool? suppres { get; set; }

    [StringLength(50)]
    public string? EmpID { get; set; }

    [StringLength(101)]
    public string? DocName { get; set; }

    public bool? AttendedToByRec { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string CoyName { get; set; } = null!;

    [StringLength(3000)]
    [Unicode(false)]
    public string? diagnosis { get; set; }

    [StringLength(3000)]
    [Unicode(false)]
    public string? diffDiagnosis { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? recDate { get; set; }

    [StringLength(50)]
    public string Remarks { get; set; } = null!;
}

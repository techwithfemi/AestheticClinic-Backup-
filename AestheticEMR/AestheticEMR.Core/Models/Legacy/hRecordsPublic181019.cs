using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hRecordsPublic181019")]
public partial class hRecordsPublic181019
{
    public int recID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime recDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(100)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    [StringLength(50)]
    public string clinicType { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? NextApptDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? htime { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? referal { get; set; }

    [StringLength(50)]
    public string? DocAssigned { get; set; }

    public bool? attendedToByDoc { get; set; }

    public byte? PatVal { get; set; }

    public bool? suppres { get; set; }

    [StringLength(50)]
    public string? DocID { get; set; }

    [StringLength(1050)]
    public string? Diagnosis { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string coyNAme { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? BillDate { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string? Mth { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? Yr { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExitDate { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? ExitDateComment { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hRecords181019")]
public partial class hRecords181019
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

    [StringLength(2)]
    public string? Mth { get; set; }

    [StringLength(30)]
    public string? Yr { get; set; }

    public DateOnly? ExitDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ExitDateComment { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? Diagnosis { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Coyname { get; set; }

    public bool? attendedToByNurse { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BillDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ConsultIDNew { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ConsultIDNew2 { get; set; }

    public bool? isJSon { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AttndStatus { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Tariff { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Debt { get; set; }

    public bool? AttendedToByImmume { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? HmoRef { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? LastConsultID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastAttndDate { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? LastClinicVisited { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? LastPurpose { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhVaccine
{
    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Date { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Time { get; set; }

    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(250)]
    public string Vaccine { get; set; } = null!;

    [StringLength(240)]
    public string? Maker { get; set; }

    [StringLength(50)]
    public string? LotNo { get; set; }

    [StringLength(50)]
    public string? BatchNo { get; set; }

    [StringLength(250)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string empID { get; set; } = null!;

    [StringLength(101)]
    public string Staff { get; set; } = null!;
}

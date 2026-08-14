using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hVaccine")]
public partial class hVaccine
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? VacDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? VacTime { get; set; }

    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(250)]
    public string Vaccine { get; set; } = null!;

    [StringLength(240)]
    public string? Manuf { get; set; }

    [StringLength(50)]
    public string? LotNo { get; set; }

    [StringLength(50)]
    public string? BatchNo { get; set; }

    [StringLength(250)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string empID { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("HistPathology")]
public partial class HistPathology
{
    [StringLength(50)]
    public string LabNo { get; set; } = null!;

    [StringLength(50)]
    public string? PathNo { get; set; }

    [StringLength(50)]
    public string? Clinician { get; set; }

    [StringLength(50)]
    public string? EtnicGroup { get; set; }

    [StringLength(50)]
    public string? Ward { get; set; }

    [StringLength(550)]
    public string Diagnosis { get; set; } = null!;

    [StringLength(1000)]
    public string? Test { get; set; }

    [StringLength(250)]
    public string? Maternal { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DtDate { get; set; }

    public string Report { get; set; } = null!;

    public long? SNoID { get; set; }

    public long ID { get; set; }
}

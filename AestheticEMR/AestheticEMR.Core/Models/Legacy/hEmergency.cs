using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hEmergency")]
public partial class hEmergency
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DtDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TimeIn { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TimeOut { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ConsultID { get; set; } = null!;

    [StringLength(5000)]
    [Unicode(false)]
    public string Complaint { get; set; } = null!;

    [StringLength(5000)]
    [Unicode(false)]
    public string Diagnosis { get; set; } = null!;

    [StringLength(5000)]
    [Unicode(false)]
    public string CareGiven { get; set; } = null!;

    [StringLength(5000)]
    [Unicode(false)]
    public string ItemsUsed { get; set; } = null!;

    [StringLength(5000)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string EmpID { get; set; } = null!;
}

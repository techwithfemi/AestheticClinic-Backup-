using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hInjection
{
    public int ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DTdATE { get; set; }

    [StringLength(50)]
    public string pno { get; set; } = null!;

    [StringLength(500)]
    public string injName { get; set; } = null!;

    [StringLength(50)]
    public string injBy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? injTime { get; set; }

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    public long? conID { get; set; }

    public long? iDNo { get; set; }

    public int? numTaken { get; set; }
}

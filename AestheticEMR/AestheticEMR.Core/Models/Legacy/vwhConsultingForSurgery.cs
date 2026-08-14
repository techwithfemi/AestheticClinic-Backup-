using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhConsultingForSurgery
{
    public long ID { get; set; }

    [StringLength(3550)]
    public string? MedRpt { get; set; }

    [StringLength(500)]
    public string ConsultID { get; set; } = null!;

    public long? ConID { get; set; }

    [StringLength(1000)]
    public string? findings { get; set; }

    [StringLength(4000)]
    public string? prosedure { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime sDate { get; set; }
}

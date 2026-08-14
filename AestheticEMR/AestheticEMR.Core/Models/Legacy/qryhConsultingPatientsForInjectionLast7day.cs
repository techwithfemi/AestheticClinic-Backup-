using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhConsultingPatientsForInjectionLast7day
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime InjDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string pSurName { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string? pFirstname { get; set; }

    public bool? attendedTo { get; set; }

    public bool? suppres { get; set; }

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    public long? conID { get; set; }
}

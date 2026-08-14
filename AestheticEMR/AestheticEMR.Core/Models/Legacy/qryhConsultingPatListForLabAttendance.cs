using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhConsultingPatListForLabAttendance
{
    public int ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime cDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string PNo { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string pSurname { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string? pFirstname { get; set; }

    public bool? attendedToByDoc { get; set; }

    [StringLength(13)]
    [Unicode(false)]
    public string Prescription { get; set; } = null!;

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(100)]
    public string? Remarks { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string treatedby { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(354)]
    public string? company { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? coyNAme { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? htime { get; set; }

    public bool? suppres { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? referal { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Ref { get; set; }

    [StringLength(150)]
    public string retainName { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string services { get; set; } = null!;
}

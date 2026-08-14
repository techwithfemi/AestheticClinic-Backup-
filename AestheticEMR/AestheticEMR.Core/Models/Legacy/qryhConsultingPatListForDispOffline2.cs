using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhConsultingPatListForDispOffline2
{
    public int ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime cDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string pSurname { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string pFirstName { get; set; } = null!;

    public bool? attendedToByPharm { get; set; }

    [StringLength(7)]
    [Unicode(false)]
    public string Prescription { get; set; } = null!;

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(100)]
    public string? Remarks { get; set; }

    [StringLength(7)]
    [Unicode(false)]
    public string treatedby { get; set; } = null!;

    public int Age { get; set; }

    [StringLength(7)]
    [Unicode(false)]
    public string company { get; set; } = null!;

    [StringLength(4)]
    [Unicode(false)]
    public string CoyName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? htime { get; set; }

    public bool? suppres { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? referal { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string RetainName { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string services { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? pPhoneNo { get; set; }
}

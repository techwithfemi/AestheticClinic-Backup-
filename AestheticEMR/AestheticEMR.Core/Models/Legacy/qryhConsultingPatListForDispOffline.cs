using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhConsultingPatListForDispOffline
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime cDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    public string pSurname { get; set; } = null!;

    [StringLength(50)]
    public string pFirstname { get; set; } = null!;

    public bool? attendedToByPharm { get; set; }

    [StringLength(3000)]
    public string? prescription { get; set; }

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    [StringLength(3000)]
    [Unicode(false)]
    public string? remarks { get; set; }

    [StringLength(101)]
    public string? treatedby { get; set; }

    public int? Age { get; set; }

    [StringLength(154)]
    public string? company { get; set; }

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? cTime { get; set; }

    [StringLength(50)]
    public string? BillRemarks { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? referal { get; set; }
}

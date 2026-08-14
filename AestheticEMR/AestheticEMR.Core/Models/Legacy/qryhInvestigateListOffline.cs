using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhInvestigateListOffline
{
    public int ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime invDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pno { get; set; } = null!;

    [StringLength(3000)]
    [Unicode(false)]
    public string? remarks { get; set; }

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    [StringLength(50)]
    public string pSurname { get; set; } = null!;

    [StringLength(50)]
    public string pFirstname { get; set; } = null!;

    [StringLength(101)]
    public string TreatedBy { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? cTime { get; set; }

    [StringLength(154)]
    public string? Company { get; set; }

    [StringLength(50)]
    public string pCatID { get; set; } = null!;

    [StringLength(2000)]
    public string? investigate { get; set; }

    public bool? attendedTobyLab { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(2000)]
    public string? invResult { get; set; }
}

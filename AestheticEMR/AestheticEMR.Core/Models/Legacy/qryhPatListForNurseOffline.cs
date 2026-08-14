using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhPatListForNurseOffline
{
    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    public string pSurname { get; set; } = null!;

    [StringLength(50)]
    public string pFirstname { get; set; } = null!;

    [StringLength(50)]
    public string? clientCat { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime recDate { get; set; }

    [StringLength(100)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string clinicType { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(154)]
    public string? company { get; set; }

    [StringLength(50)]
    public string? coyNAme { get; set; }

    public int recID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? htime { get; set; }

    public bool? suppres { get; set; }
}

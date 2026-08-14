using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhConsultingPatientsFromAttnd
{
    public int recID { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string pSurname { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string? pFirstname { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    public bool? attendedToByDoc { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime recDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? htime { get; set; }

    public bool? attendedTo { get; set; }

    public bool? suppres { get; set; }

    [StringLength(50)]
    public string clinicType { get; set; } = null!;

    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(100)]
    public string? Remarks { get; set; }

    [StringLength(251)]
    public string Fullname { get; set; } = null!;

    [StringLength(150)]
    public string retainName { get; set; } = null!;

    [StringLength(50)]
    public string? Coyname { get; set; }

    [StringLength(50)]
    public string? RetainCode { get; set; }
}

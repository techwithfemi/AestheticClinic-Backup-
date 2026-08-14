using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhInvestigateListOLD
{
    public int ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime invDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(50)]
    public string pno { get; set; } = null!;

    [StringLength(2000)]
    public string? sympItem { get; set; }

    [StringLength(2000)]
    public string? result { get; set; }

    [StringLength(400)]
    public string? remarks { get; set; }

    [StringLength(50)]
    public string? clientcat { get; set; }

    [StringLength(50)]
    public string pSurname { get; set; } = null!;

    [StringLength(50)]
    public string pFirstname { get; set; } = null!;

    public bool? attendedTo { get; set; }
}

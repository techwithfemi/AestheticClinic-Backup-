using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhInvestigateFinding
{
    public long? ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? INVDATE { get; set; }

    [StringLength(50)]
    public string? consultID { get; set; }

    [StringLength(250)]
    public string? PNO { get; set; }

    [StringLength(2000)]
    public string? sympItem { get; set; }

    [StringLength(2000)]
    public string? result { get; set; }

    [StringLength(4000)]
    public string? REMARKS { get; set; }

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    [StringLength(50)]
    public string pSurname { get; set; } = null!;

    [StringLength(50)]
    public string pFirstname { get; set; } = null!;

    public bool? ATTENDEDTO { get; set; }

    [StringLength(3)]
    public string? Capitated { get; set; }
}

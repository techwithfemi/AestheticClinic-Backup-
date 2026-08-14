using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhInPatientNotesForNurse
{
    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    public bool? isDischarged { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    public long SNo { get; set; }

    [StringLength(50)]
    public string pno { get; set; } = null!;

    [StringLength(50)]
    public string clientCat { get; set; } = null!;

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime nDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? nTime { get; set; }

    [Column(TypeName = "text")]
    public string notes { get; set; } = null!;

    [Column(TypeName = "text")]
    public string prescription { get; set; } = null!;

    [StringLength(101)]
    public string? empname { get; set; }

    [StringLength(50)]
    public string? LastName { get; set; }

    [StringLength(50)]
    public string? FirstName { get; set; }
}

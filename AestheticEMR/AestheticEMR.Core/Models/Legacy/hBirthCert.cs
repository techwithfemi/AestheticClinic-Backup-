using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hBirthCert")]
public partial class hBirthCert
{
    public long SNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOB { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TOB { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Wt { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? MothersName { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? FathersName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hPatientsPublic")]
public partial class hPatientsPublic
{
    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string pSurname { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string pFirstName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Maturity { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    public double? Debt { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string coyNAme { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? pPhoneNo { get; set; }
}

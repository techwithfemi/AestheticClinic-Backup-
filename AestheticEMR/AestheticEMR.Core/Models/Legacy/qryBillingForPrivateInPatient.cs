using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryBillingForPrivateInPatient
{
    [StringLength(50)]
    public string? drgCatGroup { get; set; }

    [StringLength(150)]
    public string? catRemarks { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    public double? sTotal { get; set; }

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(250)]
    public string diagnosis { get; set; } = null!;

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [StringLength(50)]
    public string pCatID { get; set; } = null!;
}

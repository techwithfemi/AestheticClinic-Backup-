using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwInvoiceForPatient
{
    [StringLength(50)]
    public string billNO { get; set; } = null!;

    public double? sTotal { get; set; }

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(1250)]
    public string diagnosis { get; set; } = null!;

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [StringLength(50)]
    public string? pCatID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? catRemarks { get; set; }
}

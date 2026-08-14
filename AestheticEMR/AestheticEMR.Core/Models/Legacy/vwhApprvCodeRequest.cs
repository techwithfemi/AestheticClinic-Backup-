using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhApprvCodeRequest
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RecDate { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string? ConsultID { get; set; }

    [StringLength(550)]
    public string? Remarks { get; set; }

    public bool? isSent { get; set; }

    [StringLength(50)]
    public string? RetainCode { get; set; }

    [StringLength(150)]
    public string retainName { get; set; } = null!;

    [StringLength(50)]
    public string? EnrolleeNo { get; set; }

    [StringLength(50)]
    public string? policyType { get; set; }
}

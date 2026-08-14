using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhHMOList
{
    [StringLength(50)]
    public string? empNo { get; set; }

    [StringLength(50)]
    public string? relationToStaff { get; set; }

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string pCatID { get; set; } = null!;

    [StringLength(50)]
    public string? policyType { get; set; }

    [StringLength(150)]
    public string CoyName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    [StringLength(50)]
    public string FileDuration { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhCardExpiredForPrivate
{
    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    public string? oldpNo { get; set; }

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string? clientCatID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RegDate { get; set; }

    [StringLength(50)]
    public string FileDuration { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    public bool? expired { get; set; }

    [StringLength(9)]
    public string? numval { get; set; }

    [StringLength(50)]
    public string? pCatID { get; set; }
}

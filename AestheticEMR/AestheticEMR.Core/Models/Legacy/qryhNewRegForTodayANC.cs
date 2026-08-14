using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhNewRegForTodayANC
{
    [Column(TypeName = "datetime")]
    public DateTime RegDate { get; set; }

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(101)]
    public string fullname { get; set; } = null!;

    [StringLength(50)]
    public string Sex { get; set; } = null!;

    [StringLength(50)]
    public string? CardType { get; set; }

    [StringLength(50)]
    public string pCatID { get; set; } = null!;
}

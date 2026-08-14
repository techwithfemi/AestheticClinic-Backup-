using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhAnteNatalOLD
{
    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(101)]
    public string fullname { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime ExpiryDate { get; set; }

    [StringLength(50)]
    public string? clientCatID { get; set; }
}

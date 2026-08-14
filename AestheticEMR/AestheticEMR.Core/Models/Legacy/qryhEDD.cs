using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhEDD
{
    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [Column(TypeName = "smalldatetime")]
    public DateTime? EDD { get; set; }

    public int? noOfDays { get; set; }
}

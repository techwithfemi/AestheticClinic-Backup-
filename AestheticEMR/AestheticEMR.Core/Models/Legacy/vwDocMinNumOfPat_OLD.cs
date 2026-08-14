using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDocMinNumOfPat_OLD
{
    [Column(TypeName = "datetime")]
    public DateTime? Date { get; set; }

    [StringLength(50)]
    public string? RoomNo { get; set; }

    [StringLength(101)]
    public string? DocName { get; set; }

    public int? NumOfPat { get; set; }

    public bool? IsOff { get; set; }

    [StringLength(50)]
    public string EmpID { get; set; } = null!;
}

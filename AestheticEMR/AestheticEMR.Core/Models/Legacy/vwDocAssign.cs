using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDocAssign
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(50)]
    public string RoomNo { get; set; } = null!;

    [StringLength(50)]
    public string? Location { get; set; }

    public long SNO { get; set; }

    [StringLength(50)]
    public string EmpID { get; set; } = null!;

    [StringLength(65)]
    public string? DocName { get; set; }

    public bool? IsOff { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDocWaitingListGrouped
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(65)]
    public string? Doctor { get; set; }

    [StringLength(65)]
    public string? DocName { get; set; }

    public int? NumOfPat { get; set; }

    [StringLength(50)]
    public string? empID { get; set; }

    [StringLength(50)]
    public string clinicID { get; set; } = null!;

    [StringLength(50)]
    public string ClinicName { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string RoomNo { get; set; } = null!;

    public int IsOff { get; set; }
}

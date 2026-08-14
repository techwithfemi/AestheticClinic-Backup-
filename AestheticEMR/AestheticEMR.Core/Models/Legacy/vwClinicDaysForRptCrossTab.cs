using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwClinicDaysForRptCrossTab
{
    [StringLength(50)]
    public string? CLINIC { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? MONDAY { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TUESDAY { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? WEDNESDAY { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? THURSDAY { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FRIDAY { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SATURDAY { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SUNDAY { get; set; }
}

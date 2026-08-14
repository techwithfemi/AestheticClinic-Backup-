using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwMedChartAndDressingUnion
{
    [StringLength(2500)]
    public string drgName { get; set; } = null!;

    public int? numOfTimes { get; set; }

    public int? numTaken { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;
}

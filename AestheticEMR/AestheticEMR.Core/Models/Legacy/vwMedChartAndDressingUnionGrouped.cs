using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwMedChartAndDressingUnionGrouped
{
    [StringLength(2502)]
    public string drgName { get; set; } = null!;

    public int? numTaken { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string of1 { get; set; } = null!;

    public int? numOfTimes { get; set; }

    [StringLength(6)]
    [Unicode(false)]
    public string taken { get; set; } = null!;

    [StringLength(50)]
    public string consultID { get; set; } = null!;
}

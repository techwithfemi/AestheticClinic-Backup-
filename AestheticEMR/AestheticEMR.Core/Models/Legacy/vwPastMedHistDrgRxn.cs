using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPastMedHistDrgRxn
{
    [StringLength(301)]
    [Unicode(false)]
    public string? Fullname { get; set; }

    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(4000)]
    public string? DrgRxn { get; set; }

    [StringLength(4000)]
    public string? PastMedHist { get; set; }

    [Unicode(false)]
    public string? ANCInfo { get; set; }
}

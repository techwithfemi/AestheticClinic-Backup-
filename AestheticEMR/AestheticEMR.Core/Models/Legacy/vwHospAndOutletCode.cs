using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwHospAndOutletCode
{
    [StringLength(2)]
    public string? RctCode { get; set; }

    [StringLength(8)]
    [Unicode(false)]
    public string RctSource { get; set; } = null!;
}

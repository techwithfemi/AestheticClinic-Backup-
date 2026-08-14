using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDrugCode
{
    [StringLength(50)]
    [Unicode(false)]
    public string? DrgCode { get; set; }
}

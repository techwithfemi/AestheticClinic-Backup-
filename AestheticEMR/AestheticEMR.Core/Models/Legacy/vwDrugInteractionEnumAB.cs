using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDrugInteractionEnumAB
{
    [StringLength(400)]
    [Unicode(false)]
    public string DrugA { get; set; } = null!;

    [StringLength(400)]
    [Unicode(false)]
    public string DrugB { get; set; } = null!;
}

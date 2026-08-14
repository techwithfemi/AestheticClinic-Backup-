using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("DrugInteractionEnumA")]
public partial class DrugInteractionEnumA
{
    [StringLength(400)]
    [Unicode(false)]
    public string DrugA { get; set; } = null!;
}

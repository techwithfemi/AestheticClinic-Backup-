using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("DrugInteractionEnumB")]
public partial class DrugInteractionEnumB
{
    [StringLength(400)]
    [Unicode(false)]
    public string DrugB { get; set; } = null!;
}

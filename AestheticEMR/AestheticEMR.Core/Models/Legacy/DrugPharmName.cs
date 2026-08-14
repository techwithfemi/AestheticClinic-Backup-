using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("DrugPharmName")]
public partial class DrugPharmName
{
    [StringLength(200)]
    [Unicode(false)]
    public string PharmName { get; set; } = null!;
}

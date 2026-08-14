using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("DrugPharmCat")]
public partial class DrugPharmCat
{
    [StringLength(255)]
    public string PharmCat { get; set; } = null!;
}

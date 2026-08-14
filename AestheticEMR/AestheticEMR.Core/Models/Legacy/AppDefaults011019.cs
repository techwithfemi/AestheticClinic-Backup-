using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("AppDefaults011019")]
public partial class AppDefaults011019
{
    [StringLength(50)]
    public string ID { get; set; } = null!;

    [StringLength(500)]
    public string IDVal { get; set; } = null!;
}

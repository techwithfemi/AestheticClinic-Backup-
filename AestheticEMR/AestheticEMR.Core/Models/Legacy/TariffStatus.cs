using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("TariffStatus")]
public partial class TariffStatus
{
    [StringLength(50)]
    [Unicode(false)]
    public string StatusName { get; set; } = null!;
}

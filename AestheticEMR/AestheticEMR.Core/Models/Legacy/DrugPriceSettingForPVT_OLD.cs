using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("DrugPriceSettingForPVT_OLD")]
public partial class DrugPriceSettingForPVT_OLD
{
    public long SNo { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal MinValue { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal MaxValue { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PCent { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Remarks { get; set; }
}

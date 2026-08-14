using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("DrugLocationCopy")]
public partial class DrugLocationCopy
{
    [StringLength(50)]
    public string LocID { get; set; } = null!;

    [StringLength(150)]
    public string LocName { get; set; } = null!;

    public bool? AllowEntry { get; set; }

    public bool? CanIssue { get; set; }

    public bool? isDummy { get; set; }

    public bool? isBulkCost { get; set; }
}

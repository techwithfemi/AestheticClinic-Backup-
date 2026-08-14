using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("DrugLocation")]
public partial class DrugLocation
{
    [Key]
    [StringLength(50)]
    public string LocID { get; set; } = null!;

    [StringLength(150)]
    public string LocName { get; set; } = null!;

    public bool? AllowEntry { get; set; }

    public bool? CanIssue { get; set; }

    public bool? isDummy { get; set; }

    public bool? isBulkCost { get; set; }

    public bool? isForValuation { get; set; }
}

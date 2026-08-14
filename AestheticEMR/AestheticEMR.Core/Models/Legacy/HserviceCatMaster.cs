using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("HserviceCatMaster")]
public partial class HserviceCatMaster
{
    [Key]
    [StringLength(150)]
    [Unicode(false)]
    public string MasterCatName { get; set; } = null!;

    [StringLength(100)]
    public string? Clinic { get; set; }

    [StringLength(500)]
    public string? RptHead { get; set; }
}

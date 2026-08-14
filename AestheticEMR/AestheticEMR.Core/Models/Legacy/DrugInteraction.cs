using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[PrimaryKey("DrugA", "DrugB")]
[Table("DrugInteraction")]
public partial class DrugInteraction
{
    public long SNo { get; set; }

    [Key]
    [StringLength(400)]
    [Unicode(false)]
    public string DrugA { get; set; } = null!;

    [Key]
    [StringLength(400)]
    [Unicode(false)]
    public string DrugB { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string Remarks { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string WarnLevel { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("HConsultingExamPhyGen")]
public partial class HConsultingExamPhyGen
{
    public long SNO { get; set; }

    [StringLength(250)]
    public string ItmName { get; set; } = null!;

    public int Idx { get; set; }
}

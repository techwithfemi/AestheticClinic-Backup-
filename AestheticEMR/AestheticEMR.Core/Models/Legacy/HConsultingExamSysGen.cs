using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("HConsultingExamSysGen")]
public partial class HConsultingExamSysGen
{
    public long SNO { get; set; }

    [StringLength(250)]
    public string ItmName { get; set; } = null!;

    public int Idx { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwHConsultingExam
{
    public long SNO { get; set; }

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(50)]
    public string ConID { get; set; } = null!;

    [StringLength(50)]
    public string LblDesc { get; set; } = null!;

    [StringLength(50)]
    public string LblValue { get; set; } = null!;

    [StringLength(50)]
    public string? ExamType { get; set; }
}

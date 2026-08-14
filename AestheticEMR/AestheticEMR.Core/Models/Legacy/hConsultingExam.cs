using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hConsultingExam")]
public partial class hConsultingExam
{
    public long SNO { get; set; }

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(50)]
    public string ConID { get; set; } = null!;

    [StringLength(3050)]
    public string? ExamType { get; set; }

    [StringLength(3050)]
    public string LblDesc { get; set; } = null!;

    [StringLength(3050)]
    public string LblValue { get; set; } = null!;
}

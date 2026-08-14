using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hConsultingMedRpt")]
public partial class hConsultingMedRpt
{
    public long SNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ConsultID { get; set; } = null!;

    [Unicode(false)]
    public string MedReport { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }
}

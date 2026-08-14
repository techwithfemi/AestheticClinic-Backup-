using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hReferalTest")]
public partial class hReferalTest
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? apptDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? apptTime { get; set; }

    [StringLength(50)]
    public string? pNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? refDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? refTime { get; set; }
}

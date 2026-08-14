using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hApptTest")]
public partial class hApptTest
{
    public long ID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string pno { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? entryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? entryTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApptDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApptTime { get; set; }
}

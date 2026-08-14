using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPOrderApprvGen
{
    [StringLength(50)]
    public string POID { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    public long ID { get; set; }
}

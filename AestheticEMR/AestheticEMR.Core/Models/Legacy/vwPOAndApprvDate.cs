using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPOAndApprvDate
{
    [StringLength(50)]
    public string POID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OrderDateApprv { get; set; }
}

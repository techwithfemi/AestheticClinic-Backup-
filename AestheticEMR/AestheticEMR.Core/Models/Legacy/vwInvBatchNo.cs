using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwInvBatchNo
{
    [Column(TypeName = "datetime")]
    public DateTime InvDate { get; set; }

    [StringLength(50)]
    public string? BatchNo { get; set; }

    [StringLength(50)]
    public string? CoyCode { get; set; }

    [StringLength(150)]
    public string retainName { get; set; } = null!;
}

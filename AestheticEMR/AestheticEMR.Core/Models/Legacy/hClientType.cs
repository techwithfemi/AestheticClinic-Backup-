using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("hClientType")]
public partial class hClientType
{
    [Key]
    [StringLength(50)]
    public string ClientType { get; set; } = null!;

    public long SNo { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("HospList")]
public partial class HospList
{
    [StringLength(50)]
    public string HospID { get; set; } = null!;

    [StringLength(150)]
    public string? HName { get; set; }

    [StringLength(150)]
    public string? HName2 { get; set; }

    public long SNo { get; set; }
}

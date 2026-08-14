using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hClientScheme")]
public partial class hClientScheme
{
    [StringLength(50)]
    public string ClientType { get; set; } = null!;

    [StringLength(50)]
    public string? SchemeID { get; set; }

    [StringLength(50)]
    public string? SchemeName { get; set; }
}

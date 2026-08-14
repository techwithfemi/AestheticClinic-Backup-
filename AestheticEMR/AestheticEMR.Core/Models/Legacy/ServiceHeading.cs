using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class ServiceHeading
{
    [StringLength(100)]
    [Unicode(false)]
    public string Category { get; set; } = null!;

    [StringLength(500)]
    public string? Heading { get; set; }

    public long? AcctID { get; set; }

    public long? AcctIDVal { get; set; }
}

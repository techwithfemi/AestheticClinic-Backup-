using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryService
{
    public long? AcctID { get; set; }

    [StringLength(50)]
    public string? Code { get; set; }

    [StringLength(500)]
    public string? CodeItem { get; set; }

    [StringLength(500)]
    public string? Heading { get; set; }

    [StringLength(500)]
    public string? Category { get; set; }

    [StringLength(500)]
    public string? Remarks { get; set; }

    public bool? isHeading { get; set; }

    public long? AcctIDVal { get; set; }

    [StringLength(558)]
    public string? CodeAndItem { get; set; }
}

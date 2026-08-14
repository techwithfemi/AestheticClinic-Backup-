using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hFamAndSocHist")]
public partial class hFamAndSocHist
{
    [StringLength(50)]
    public string pno { get; set; } = null!;

    [StringLength(400)]
    public string? fHist { get; set; }

    [StringLength(400)]
    public string? sHist { get; set; }
}

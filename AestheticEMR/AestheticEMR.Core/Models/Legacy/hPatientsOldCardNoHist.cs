using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hPatientsOldCardNoHist")]
public partial class hPatientsOldCardNoHist
{
    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [StringLength(50)]
    public string OldCardNo { get; set; } = null!;

    [StringLength(50)]
    public string CardYear { get; set; } = null!;
}

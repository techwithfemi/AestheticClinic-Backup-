using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hhScreeningResultDetail")]
public partial class hhScreeningResultDetail
{
    public long IndexNo { get; set; }

    [StringLength(50)]
    public string LABNO { get; set; } = null!;

    [StringLength(50)]
    public string TagName { get; set; } = null!;

    [StringLength(1500)]
    public string lblDesc { get; set; } = null!;

    [StringLength(1500)]
    public string Result { get; set; } = null!;

    [StringLength(50)]
    public string Range { get; set; } = null!;

    [StringLength(50)]
    public string Units { get; set; } = null!;

    public long? SNoID { get; set; }
}

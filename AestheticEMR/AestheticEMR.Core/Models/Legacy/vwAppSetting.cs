using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwAppSetting
{
    [StringLength(50)]
    public string ID { get; set; } = null!;

    [StringLength(550)]
    public string? IDVal { get; set; }

    [StringLength(550)]
    public string? IDVal2 { get; set; }

    [StringLength(1150)]
    public string? IDVal3 { get; set; }

    [StringLength(500)]
    public string? IDVal4 { get; set; }

    [StringLength(50)]
    public string? IDVal5 { get; set; }

    [StringLength(50)]
    public string? IDValPix { get; set; }

    [StringLength(2)]
    public string? IDValCode { get; set; }

    [StringLength(50)]
    public string ClinicID { get; set; } = null!;

    [StringLength(100)]
    public string ClinicName { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhConReferal
{
    [StringLength(50)]
    public string psurname { get; set; } = null!;

    [StringLength(50)]
    public string pfirstname { get; set; } = null!;

    [StringLength(50)]
    public string consultid { get; set; } = null!;

    public bool? attendedto { get; set; }
}

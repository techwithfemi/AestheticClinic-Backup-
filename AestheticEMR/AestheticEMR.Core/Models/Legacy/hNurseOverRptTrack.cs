using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hNurseOverRptTrack")]
public partial class hNurseOverRptTrack
{
    public long SNo { get; set; }

    public long SNoID { get; set; }

    public string? Details { get; set; }
}

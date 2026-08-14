using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hAttendanceSummDate
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DtDAte { get; set; }
}

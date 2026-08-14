using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qrySysDateTime
{
    [Column(TypeName = "datetime")]
    public DateTime sysDT { get; set; }
}

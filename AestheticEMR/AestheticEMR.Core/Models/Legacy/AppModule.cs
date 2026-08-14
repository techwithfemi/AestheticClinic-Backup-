using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class AppModule
{
    [StringLength(2)]
    [Unicode(false)]
    public string ModID { get; set; } = null!;

    [StringLength(50)]
    public string ModName { get; set; } = null!;
}

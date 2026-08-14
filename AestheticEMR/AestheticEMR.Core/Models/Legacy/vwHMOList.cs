using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwHMOList
{
    [StringLength(50)]
    public string retainID { get; set; } = null!;

    [StringLength(150)]
    public string retainName { get; set; } = null!;
}

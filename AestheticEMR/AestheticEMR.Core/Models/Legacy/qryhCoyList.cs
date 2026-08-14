using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhCoyList
{
    [StringLength(50)]
    public string clientCatID { get; set; } = null!;

    [StringLength(150)]
    public string ClientCatName { get; set; } = null!;
}

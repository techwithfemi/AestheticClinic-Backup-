using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class DesignationQry
{
    [StringLength(50)]
    public string desID { get; set; } = null!;

    [StringLength(100)]
    public string desName { get; set; } = null!;
}

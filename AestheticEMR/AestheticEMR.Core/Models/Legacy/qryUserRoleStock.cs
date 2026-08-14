using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryUserRoleStock
{
    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [StringLength(65)]
    public string? Fullname { get; set; }

    [StringLength(10)]
    public string? AccountStatus { get; set; }

    [StringLength(50)]
    public string LocID { get; set; } = null!;

    [StringLength(150)]
    public string LocName { get; set; } = null!;
}

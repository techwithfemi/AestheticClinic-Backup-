using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("GroupBank")]
public partial class GroupBank
{
    [StringLength(3)]
    public string GroupCode { get; set; } = null!;

    [StringLength(225)]
    public string GroupName { get; set; } = null!;
}

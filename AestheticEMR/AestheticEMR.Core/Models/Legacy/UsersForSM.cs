using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("UsersForSMS")]
public partial class UsersForSM
{
    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;
}

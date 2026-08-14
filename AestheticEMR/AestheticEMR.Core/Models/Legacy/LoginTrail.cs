using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("LoginTrail")]
public partial class LoginTrail
{
    [StringLength(50)]
    public string? SerialNo { get; set; }

    [StringLength(18)]
    public string? Username { get; set; }

    [StringLength(25)]
    public string? LoginType { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LogDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LogTime { get; set; }
}

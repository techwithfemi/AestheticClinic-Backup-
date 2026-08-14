using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("LogInTray")]
public partial class LogInTray
{
    public int ID { get; set; }

    public int? UserID { get; set; }

    [StringLength(50)]
    public string? FullName { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LogInTime { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? LogOutTime { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? Date { get; set; }

    [StringLength(50)]
    public string? RemoteMachine { get; set; }
}

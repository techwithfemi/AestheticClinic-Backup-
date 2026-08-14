using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class UserActivity
{
    public long SNO { get; set; }

    [StringLength(50)]
    public string Username { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime LoginDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime LoginTime { get; set; }

    public bool? IsLogOut { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LogOutDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LogOutTime { get; set; }

    public bool? AutoLogoff { get; set; }

    [StringLength(50)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }

    public int? AppVersion { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("WatchDog_WatchExceptionLog")]
public partial class WatchDog_WatchExceptionLog
{
    [Key]
    public int id { get; set; }

    [Unicode(false)]
    public string? message { get; set; }

    [Unicode(false)]
    public string? stackTrace { get; set; }

    [Unicode(false)]
    public string? typeOf { get; set; }

    [Unicode(false)]
    public string? source { get; set; }

    [Unicode(false)]
    public string? path { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? method { get; set; }

    [Unicode(false)]
    public string? queryString { get; set; }

    [Unicode(false)]
    public string? requestBody { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string encounteredAt { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("WatchDog_WatchLog")]
public partial class WatchDog_WatchLog
{
    [Key]
    public int id { get; set; }

    [Unicode(false)]
    public string? responseBody { get; set; }

    public int responseStatus { get; set; }

    [Unicode(false)]
    public string? requestBody { get; set; }

    [Unicode(false)]
    public string? queryString { get; set; }

    [Unicode(false)]
    public string? path { get; set; }

    [Unicode(false)]
    public string? requestHeaders { get; set; }

    [Unicode(false)]
    public string? responseHeaders { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? method { get; set; }

    [Unicode(false)]
    public string? host { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? ipAddress { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? timeSpent { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string startTime { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string endTime { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

public partial class WatchDog_Log
{
    [Key]
    public int id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? eventId { get; set; }

    [Unicode(false)]
    public string? message { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string timestamp { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? callingFrom { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? callingMethod { get; set; }

    public int? lineNumber { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? logLevel { get; set; }
}

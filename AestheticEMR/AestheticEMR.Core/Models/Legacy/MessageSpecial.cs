using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("MessageSpecial")]
public partial class MessageSpecial
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime SendDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SendTime { get; set; }

    [StringLength(5000)]
    [Unicode(false)]
    public string? Message { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string CatName { get; set; } = null!;

    [StringLength(5000)]
    [Unicode(false)]
    public string? Remarks { get; set; }
}

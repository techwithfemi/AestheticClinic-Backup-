using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwSurgeryAnaesthNote
{
    [StringLength(500)]
    public string ConsultID { get; set; } = null!;

    public long? ConID { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? AnaesthNotePre { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? AnaesthNotePost { get; set; }
}

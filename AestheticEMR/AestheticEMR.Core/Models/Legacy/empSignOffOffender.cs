using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class empSignOffOffender
{
    public long RecID { get; set; }

    [StringLength(50)]
    public string StaffNo { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime SignInDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime SignInTime { get; set; }
}

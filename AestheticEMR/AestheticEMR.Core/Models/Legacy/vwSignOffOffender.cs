using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwSignOffOffender
{
    public long recID { get; set; }

    [StringLength(50)]
    public string StaffNo { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime SignInDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime SignInTime { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Shift { get; set; } = null!;
}

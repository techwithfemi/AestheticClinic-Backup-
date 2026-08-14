using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwClinicTypesForSM
{
    [StringLength(100)]
    public string ClinicName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Designation { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PhoneNo { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClinicPeriod { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Apologies { get; set; }
}

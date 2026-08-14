using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("ProposalInfo")]
public partial class ProposalInfo
{
    public long SNo { get; set; }

    [StringLength(1000)]
    public string CoyName { get; set; } = null!;

    [StringLength(1000)]
    public string Address { get; set; } = null!;

    [StringLength(50)]
    public string Phone { get; set; } = null!;

    [StringLength(50)]
    public string Email { get; set; } = null!;

    [StringLength(500)]
    public string ContactName { get; set; } = null!;

    [StringLength(50)]
    public string Designation { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwHospDocInfoPublic
{
    [StringLength(3)]
    public string CoyID { get; set; } = null!;

    [StringLength(225)]
    public string CoyName { get; set; } = null!;

    [StringLength(7)]
    public string DocID { get; set; } = null!;

    [StringLength(255)]
    public string DocName { get; set; } = null!;

    [StringLength(255)]
    public string? Branch { get; set; }

    [StringLength(250)]
    public string? Location { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }

    [StringLength(50)]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? AcctNo { get; set; }

    [StringLength(10)]
    public string AcctID { get; set; } = null!;
}

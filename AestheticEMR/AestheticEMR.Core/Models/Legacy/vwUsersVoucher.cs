using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwUsersVoucher
{
    [StringLength(101)]
    public string? Fullname { get; set; }

    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [StringLength(10)]
    public string? AccountStatus { get; set; }

    [StringLength(18)]
    public string? LoginRole { get; set; }

    [StringLength(50)]
    public string? Privilege { get; set; }

    [StringLength(2)]
    public string? SetID { get; set; }

    [StringLength(50)]
    public string EmpID { get; set; } = null!;
}

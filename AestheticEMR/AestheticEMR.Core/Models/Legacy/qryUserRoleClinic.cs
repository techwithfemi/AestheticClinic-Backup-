using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryUserRoleClinic
{
    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [StringLength(65)]
    public string? Fullname { get; set; }

    [StringLength(65)]
    public string? Doctor { get; set; }

    [StringLength(10)]
    public string? AccountStatus { get; set; }

    [StringLength(50)]
    public string clinicID { get; set; } = null!;

    [StringLength(100)]
    public string ClinicName { get; set; } = null!;

    [StringLength(50)]
    public string EmpID { get; set; } = null!;
}

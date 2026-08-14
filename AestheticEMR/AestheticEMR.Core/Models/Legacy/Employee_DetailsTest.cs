using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("Employee_DetailsTest")]
public partial class Employee_DetailsTest
{
    public long UserID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Username { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Firstname { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Lastname { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? City { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Designation { get; set; }
}

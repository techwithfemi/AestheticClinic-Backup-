using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("EmpLoanCat")]
public partial class EmpLoanCat
{
    [StringLength(50)]
    public string LoanCatID { get; set; } = null!;

    [StringLength(50)]
    public string? LoanName { get; set; }
}

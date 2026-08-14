using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwAcctNo_Payable_Expense
{
    [StringLength(50)]
    [Unicode(false)]
    public string ID { get; set; } = null!;

    [StringLength(250)]
    [Unicode(false)]
    public string IDVal { get; set; } = null!;
}

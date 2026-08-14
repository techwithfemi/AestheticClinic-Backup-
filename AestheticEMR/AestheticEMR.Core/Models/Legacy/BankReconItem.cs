using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class BankReconItem
{
    public int SNO { get; set; }

    [StringLength(50)]
    public string BankItem { get; set; } = null!;

    [StringLength(50)]
    public string? Status { get; set; }

    [StringLength(50)]
    public string? Remarks { get; set; }
}

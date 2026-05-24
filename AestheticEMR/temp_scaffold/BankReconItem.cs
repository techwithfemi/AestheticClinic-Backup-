using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class BankReconItem
{
    public int Sno { get; set; }

    public string BankItem { get; set; } = null!;

    public string? Status { get; set; }

    public string? Remarks { get; set; }
}

using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhInvestigateFindingsOld
{
    public int Id { get; set; }

    public DateTime InvDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string? SympItem { get; set; }

    public string? Result { get; set; }

    public string? Remarks { get; set; }

    public string? Clientcat { get; set; }

    public string PSurname { get; set; } = null!;

    public string PFirstname { get; set; } = null!;
}

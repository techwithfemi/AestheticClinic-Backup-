using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillAccumUnAttndTo
{
    public string Fullname { get; set; } = null!;

    public string Company { get; set; } = null!;

    public DateTime Date { get; set; }

    public string? ClientCatId { get; set; }

    public string ClinicType { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string? Diagnosis { get; set; }

    public decimal? SubTotal { get; set; }

    public bool? IsBilled { get; set; }

    public DateTime? Time { get; set; }
}

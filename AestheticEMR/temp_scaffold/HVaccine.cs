using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HVaccine
{
    public long Id { get; set; }

    public DateTime? VacDate { get; set; }

    public DateTime? VacTime { get; set; }

    public string Pno { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string Vaccine { get; set; } = null!;

    public string? Manuf { get; set; }

    public string? LotNo { get; set; }

    public string? BatchNo { get; set; }

    public string? Remarks { get; set; }

    public string EmpId { get; set; } = null!;
}

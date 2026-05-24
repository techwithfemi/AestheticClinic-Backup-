using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HFluidChart
{
    public long Sno { get; set; }

    public string Pno { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string? ClientCat { get; set; }

    public string? Oral { get; set; }

    public string? Intrav { get; set; }

    public string? Intothers { get; set; }

    public decimal? IntVol { get; set; }

    public string? IntSod { get; set; }

    public string? IntPot { get; set; }

    public string? Urine { get; set; }

    public string? VomitusAspirate { get; set; }

    public string? OutOthers { get; set; }

    public decimal? OutVol { get; set; }

    public string? OutSod { get; set; }

    public string? OutPot { get; set; }

    public string EmpId { get; set; } = null!;

    public DateTime Ntime { get; set; }

    public DateTime NDate { get; set; }

    public long? ConId { get; set; }

    public string? IntakeFluid { get; set; }

    public string? IntakeFluidType { get; set; }

    public string? OutPutFluid { get; set; }

    public string? OutputFluidType { get; set; }

    public string? ChartTime { get; set; }
}

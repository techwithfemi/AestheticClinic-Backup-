using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhConsultingDrugsForEdit
{
    public long Id { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime DtDate { get; set; }

    public string DrgName { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public double Qty { get; set; }

    public string? Usage { get; set; }

    public bool? AttendedTo { get; set; }

    public string? ConId { get; set; }

    public double Price { get; set; }

    public double SubTotal { get; set; }

    public string Billtype { get; set; } = null!;
}

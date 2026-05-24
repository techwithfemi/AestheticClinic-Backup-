using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhConsultingServiceForEdit
{
    public string Service { get; set; } = null!;

    public string Category { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public double SubTotal { get; set; }

    public string Description { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? ConId { get; set; }

    public string Billtype { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string ConsultId { get; set; } = null!;
}

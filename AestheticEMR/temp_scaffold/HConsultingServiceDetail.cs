using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HConsultingServiceDetail
{
    public long Sno { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime ServDate { get; set; }

    public DateTime? ServTime { get; set; }

    public string ServName { get; set; } = null!;

    public decimal? Qty { get; set; }

    public decimal? Price { get; set; }

    public decimal? Subtotal { get; set; }

    public string? Description { get; set; }

    public bool? AttendedTo { get; set; }

    public string? ConId { get; set; }

    public bool? Isdone { get; set; }

    public string? EmpId { get; set; }
}

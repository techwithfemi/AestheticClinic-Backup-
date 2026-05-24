using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhConsultingLabForEditStd
{
    public int Id { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime InvDate { get; set; }

    public string? SympItem { get; set; }

    public string? SympItemCat { get; set; }

    public string? ConId { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? IsLab { get; set; }

    public double? Price { get; set; }

    public double? Qty { get; set; }

    public double? SubTotal { get; set; }
}

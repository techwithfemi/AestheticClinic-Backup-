using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhConsultingLabForEdit
{
    public int Id { get; set; }

    public string ConsultId { get; set; } = null!;

    public DateTime InvDate { get; set; }

    public string? SympItem { get; set; }

    public string? SympItemCat { get; set; }

    public double Qty { get; set; }

    public string? ConId { get; set; }

    public double Price { get; set; }

    public double SubTotal { get; set; }

    public string Billtype { get; set; } = null!;

    public bool? AttendedTo { get; set; }
}

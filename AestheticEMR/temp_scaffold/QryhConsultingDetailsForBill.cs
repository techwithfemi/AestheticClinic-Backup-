using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultingDetailsForBill
{
    public string ConsultId { get; set; } = null!;

    public DateTime DtDate { get; set; }

    public string DrgName { get; set; } = null!;

    public double Qty { get; set; }

    public double Price { get; set; }

    public double TotPrice { get; set; }
}

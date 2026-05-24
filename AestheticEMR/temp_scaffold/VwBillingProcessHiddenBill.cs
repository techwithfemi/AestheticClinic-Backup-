using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingProcessHiddenBill
{
    public DateTime Date { get; set; }

    public DateTime? Time { get; set; }

    public DateTime? BillDate { get; set; }

    public string Fullname { get; set; } = null!;

    public string Service { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal Qty { get; set; }

    public decimal? SubTotal { get; set; }

    public string ClinicType { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public string RetainCode { get; set; } = null!;

    public string? Remarks { get; set; }

    public string? Mth { get; set; }

    public string? Yr { get; set; }

    public string? BatchVal { get; set; }

    public string? BatchNo { get; set; }

    public string? MonthCode { get; set; }

    public string PNo { get; set; } = null!;

    public string RetainId { get; set; } = null!;

    public DateTime? AttndBillDate { get; set; }

    public bool? IsBilled { get; set; }

    public string? PhoneNo { get; set; }
}

using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhRecordsAndBill
{
    public int RecId { get; set; }

    public DateTime RecDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string? ClientCat { get; set; }

    public string? Remarks { get; set; }

    public string? EmpId { get; set; }

    public string ClinicType { get; set; } = null!;

    public DateTime? Htime { get; set; }

    public string? Mth { get; set; }

    public string? Yr { get; set; }

    public string? BatchNo { get; set; }

    public string? Coyname { get; set; }

    public DateTime? BillDate { get; set; }

    public string RetainCode { get; set; } = null!;

    public string RetainName { get; set; } = null!;

    public string? ClientCatId { get; set; }

    public string? ClientType { get; set; }

    public string Fullname { get; set; } = null!;

    public string? MonthName { get; set; }

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }

    public string? PhoneNo { get; set; }

    public decimal? Discount { get; set; }

    public decimal? DebtBf { get; set; }
}

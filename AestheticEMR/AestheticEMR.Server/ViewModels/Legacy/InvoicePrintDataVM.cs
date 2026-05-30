namespace AestheticEMR.Server.ViewModels.Legacy;

public class InvoicePrintDataVM
{
    // Header (from emrAppDefaults PublicVariables)
    public string BillHead { get; set; } = string.Empty;
    public string BillHead2 { get; set; } = string.Empty;
    public string BillHead3 { get; set; } = string.Empty;
    public string BillHead4 { get; set; } = string.Empty;

    // Invoice metadata
    public string BillNo { get; set; } = string.Empty;
    public string BillDate { get; set; } = string.Empty;
    public string TaxName { get; set; } = string.Empty;
    public string TIN { get; set; } = string.Empty;
    public double TaxPcent { get; set; }

    // Patient info
    public string PatientName { get; set; } = string.Empty;
    public string PatientNo { get; set; } = string.Empty;
    public string ClientCat { get; set; } = string.Empty;   // e.g. PRIVATE, HMO

    // Company/payer address (the company the patient belongs to, or clinic address for PRIVATE)
    public string PayerName { get; set; } = string.Empty;
    public string PayerAddress { get; set; } = string.Empty;
    public string PayerPhone { get; set; } = string.Empty;

    // Summary (from billing table)
    public decimal DebtBF { get; set; }
    public decimal AmountBilled { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal Balance { get; set; }

    // Line items (from billingDetails table)
    public List<InvoicePrintDetailVM> Details { get; set; } = [];
}

public class InvoicePrintDetailVM
{
    public long Sno { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public double Price { get; set; }
    public double Qty { get; set; }
    public decimal SubTotal { get; set; }
    public string? Category { get; set; }
    public string? BillType { get; set; }
}

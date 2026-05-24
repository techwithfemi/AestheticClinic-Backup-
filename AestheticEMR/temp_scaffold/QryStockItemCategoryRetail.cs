using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryStockItemCategoryRetail
{
    public string CategoryCode { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }
}

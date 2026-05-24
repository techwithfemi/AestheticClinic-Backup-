using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class AppProductCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Icon { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<AppProduct> AppProducts { get; set; } = new List<AppProduct>();
}

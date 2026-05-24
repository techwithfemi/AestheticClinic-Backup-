using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class DentalImaging
{
    public int Id { get; set; }

    public string Pno { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public DateTime ImagingDate { get; set; }

    public string? ImagingType { get; set; }

    public string? ToothRegion { get; set; }

    public string? Findings { get; set; }

    public string? Impression { get; set; }

    public string? Recommendations { get; set; }

    public string? FilePath { get; set; }

    public string? FileName { get; set; }

    public string? Notes { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }
}

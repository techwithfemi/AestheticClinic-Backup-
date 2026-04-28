using AestheticEMR.Core.Models;

namespace AestheticEMR.Core.Models.Dental;

public class DentalImaging : BaseEntity
{
    public required string Pno { get; set; }
    public required string ConsultId { get; set; }
    public DateTime ImagingDate { get; set; }
    public string? ImagingType { get; set; }       // e.g. Periapical, Panoramic, Bitewing, CBCT
    public string? ToothRegion { get; set; }
    public string? Findings { get; set; }
    public string? Impression { get; set; }
    public string? Recommendations { get; set; }
    public string? FilePath { get; set; }
    public string? FileName { get; set; }
    public string? Notes { get; set; }
}

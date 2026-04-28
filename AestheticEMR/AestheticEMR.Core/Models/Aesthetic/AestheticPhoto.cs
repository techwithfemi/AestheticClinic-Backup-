// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AestheticEMR.Core.Models;

namespace AestheticEMR.Core.Models.Aesthetic
{
    public class AestheticPhoto : BaseEntity
    {
        public required int ConsultationId { get; set; }
        public required AestheticConsultation Consultation { get; set; }
        public required string FileName { get; set; }
        public required string FilePath { get; set; }
        public string? Type { get; set; }
        public string? ConsultId { get; set; }
        public string? PNo { get; set; }
    }
}

// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

namespace AestheticEMR.Server.ViewModels.Aesthetic
{
    public class AestheticPhotoVM
    {
        public int Id { get; set; }
        public int ConsultationId { get; set; }
        public string? ConsultId { get; set; }
        public string? PNo { get; set; }
        public string? FileName { get; set; }
        public string? Type { get; set; }
        public string? Url { get; set; }
        public string? ThumbnailUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}

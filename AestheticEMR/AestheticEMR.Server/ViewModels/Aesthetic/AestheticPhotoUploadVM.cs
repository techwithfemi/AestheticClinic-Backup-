using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AestheticEMR.Server.ViewModels.Aesthetic
{
    public class AestheticPhotoUploadVM
    {
        [Range(1, int.MaxValue)]
        public int ConsultationId { get; set; }

        [Required]
        public IFormFile File { get; set; } = default!;

        [StringLength(50)]
        public string? Type { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Legacy;

public class UpdateDiscountVM
{
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Discount must be a non-negative value.")]
    public decimal Discount { get; set; }
}

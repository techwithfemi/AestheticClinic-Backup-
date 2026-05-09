using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Shop;

public class ProductCategoryEditVM
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(256)]
    public string? Icon { get; set; }
}

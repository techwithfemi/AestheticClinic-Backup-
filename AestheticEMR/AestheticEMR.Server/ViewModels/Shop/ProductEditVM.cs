using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Shop;

public class ProductEditVM
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(256)]
    public string? Icon { get; set; }

    [Range(0, (double)decimal.MaxValue)]
    public decimal BuyingPrice { get; set; }

    [Range(0, int.MaxValue)]
    public int UnitsInStock { get; set; }

    public bool IsActive { get; set; }

    public bool IsDiscontinued { get; set; }

    [Range(1, int.MaxValue)]
    public int ProductCategoryId { get; set; }
}

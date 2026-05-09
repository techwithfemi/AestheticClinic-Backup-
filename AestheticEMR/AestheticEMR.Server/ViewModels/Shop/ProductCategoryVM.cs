namespace AestheticEMR.Server.ViewModels.Shop;

public class ProductCategoryVM
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Icon { get; set; }
}

// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AestheticEMR.Core.Models.Shop;

namespace AestheticEMR.Core.Services.Shop
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product, string? userName);
        Task<Product> UpdateAsync(Product product, string? userName);
        Task DeleteAsync(int id);
        Task<IEnumerable<ProductStockReport>> GetStockReportAsync();

        Task<IEnumerable<ProductCategory>> GetCategoriesAsync();
        Task<ProductCategory?> GetCategoryByIdAsync(int id);
        Task<ProductCategory> CreateCategoryAsync(ProductCategory category);
        Task<ProductCategory> UpdateCategoryAsync(ProductCategory category);
        Task DeleteCategoryAsync(int id);
    }
}

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
        Task<int> UploadAsync(Stream fileStream, string fileName, int itemColumn, int qtyColumn, bool deleteExisting, string? userName, string? sheetName = null);

        Task<IEnumerable<ProductCategory>> GetCategoriesAsync();
        Task<ProductCategory?> GetCategoryByIdAsync(int id);
        Task<ProductCategory> CreateCategoryAsync(ProductCategory category);
        Task<ProductCategory> UpdateCategoryAsync(ProductCategory category);
        Task DeleteCategoryAsync(int id);

        Task<IEnumerable<ProductBatch>> GetBatchesAsync(int? productId = null, bool includeRecalled = false);
        Task<ProductBatch?> GetBatchByIdAsync(int id);
        Task<ProductBatch> CreateBatchAsync(ProductBatch batch, string? userName);
        Task<ProductBatch> RecallBatchAsync(int id, string reason, string? userName);
        Task<IEnumerable<ProductBatch>> GetExpiringBatchesAsync(int daysAhead = 30);

        Task<ProcedureProductUsage> RecordProcedureUsageAsync(ProcedureProductUsage usage, string? userName);
        Task<IEnumerable<ProcedureProductUsage>> GetProcedureUsagesAsync(int? consultationId = null, int? productId = null);
    }
}

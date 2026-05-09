// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Shop;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core.Services.Shop
{
    public class ProductService(ApplicationDbContext context) : IProductService
    {
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await context.Products
                .Include(x => x.ProductCategory)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await context.Products
                .Include(x => x.ProductCategory)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product> CreateAsync(Product product, string? userName)
        {
            product.PreviousBuyingPrices = 0;
            product.PreviousSellingPrice = 0;
            product.PreviousUnitsInStock = 0;

            context.Products.Add(product);

            var hasStockValueChanges = product.BuyingPrice != 0
                || product.SellingPrice != 0
                || product.UnitsInStock != 0;

            if (hasStockValueChanges)
            {
                AddStockReportEntry(product, "Create", userName);
            }

            await context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateAsync(Product product, string? userName)
        {
            var currentValues = await context.Products
                .AsNoTracking()
                .Where(x => x.Id == product.Id)
                .Select(x => new
                {
                    x.BuyingPrice,
                    x.SellingPrice,
                    x.UnitsInStock,
                    x.PreviousBuyingPrices,
                    x.PreviousSellingPrice,
                    x.PreviousUnitsInStock
                })
                .FirstOrDefaultAsync();

            if (currentValues is null)
            {
                await context.SaveChangesAsync();
                return product;
            }

            var hasStockValueChanges = product.BuyingPrice != currentValues.BuyingPrice
                || product.SellingPrice != currentValues.SellingPrice
                || product.UnitsInStock != currentValues.UnitsInStock;

            if (hasStockValueChanges)
            {
                product.PreviousBuyingPrices = currentValues.BuyingPrice;
                product.PreviousSellingPrice = currentValues.SellingPrice;
                product.PreviousUnitsInStock = currentValues.UnitsInStock;

                AddStockReportEntry(product, "Update", userName);
            }
            else
            {
                product.PreviousBuyingPrices = currentValues.PreviousBuyingPrices;
                product.PreviousSellingPrice = currentValues.PreviousSellingPrice;
                product.PreviousUnitsInStock = currentValues.PreviousUnitsInStock;
            }

            await context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product is null)
            {
                return;
            }

            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductStockReport>> GetStockReportAsync()
        {
            return await context.ProductStockReports
                .Include(x => x.Product)
                .OrderByDescending(x => x.OperationTimestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductCategory>> GetCategoriesAsync()
        {
            return await context.ProductCategories
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<ProductCategory?> GetCategoryByIdAsync(int id)
        {
            return await context.ProductCategories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ProductCategory> CreateCategoryAsync(ProductCategory category)
        {
            context.ProductCategories.Add(category);
            await context.SaveChangesAsync();
            return category;
        }

        public async Task<ProductCategory> UpdateCategoryAsync(ProductCategory category)
        {
            await context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await context.ProductCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (category is null)
            {
                return;
            }

            var isUsed = await context.Products.AnyAsync(x => x.ProductCategoryId == id);
            if (isUsed)
            {
                throw new InvalidOperationException("This category is in use by existing products and cannot be deleted.");
            }

            context.ProductCategories.Remove(category);
            await context.SaveChangesAsync();
        }

        private void AddStockReportEntry(Product product, string operationType, string? userName)
        {
            var now = DateTime.UtcNow;

            context.ProductStockReports.Add(new ProductStockReport
            {
                ProductId = product.Id,
                Product = product,
                OperationType = operationType,
                BuyingPrice = product.BuyingPrice,
                SellingPrice = product.SellingPrice,
                PreviousBuyingPrices = product.PreviousBuyingPrices,
                PreviousSellingPrice = product.PreviousSellingPrice,
                PreviousUnitsInStock = product.PreviousUnitsInStock,
                UnitsInStock = product.UnitsInStock,
                OperationDate = now.Date,
                OperationTime = now,
                OperationTimestamp = now,
                UserName = userName
            });
        }
    }
}

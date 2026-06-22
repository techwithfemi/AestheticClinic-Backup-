// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Shop;
using Microsoft.EntityFrameworkCore;
using ExcelDataReader;
using System.Data;
using System.Globalization;
using System.Text;

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

        public async Task<Product?> GetByNameAsync(string name)
        {
            var normalizedName = (name ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(normalizedName))
            {
                return null;
            }

            return await context.Products
                .Include(x => x.ProductCategory)
                .FirstOrDefaultAsync(x => x.Name.ToLower() == normalizedName.ToLower());
        }

        public async Task<Product> CreateAsync(Product product, string? userName)
        {
            // Load the category to satisfy the 'required' constraint
            product.ProductCategory = await context.ProductCategories
                .FirstOrDefaultAsync(x => x.Id == product.ProductCategoryId)
                ?? throw new InvalidOperationException($"Category with ID {product.ProductCategoryId} not found");

            product.PreviousBuyingPrices = 0;
            product.PreviousSellingPrice = 0;
            product.PreviousUnitsInStock = 0;

            context.Products.Add(product);

            var hasStockValueChanges = product.BuyingPrice != 0
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
            // Load the category to satisfy the 'required' constraint
            product.ProductCategory = await context.ProductCategories
                .FirstOrDefaultAsync(x => x.Id == product.ProductCategoryId)
                ?? throw new InvalidOperationException($"Category with ID {product.ProductCategoryId} not found");

            var currentValues = await context.Products
                .AsNoTracking()
                .Where(x => x.Id == product.Id)
                .Select(x => new
                {
                    x.BuyingPrice,
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
                || product.UnitsInStock != currentValues.UnitsInStock;

            if (hasStockValueChanges)
            {
                product.PreviousBuyingPrices = currentValues.BuyingPrice;
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

        public async Task<IEnumerable<ProductBatch>> GetBatchesAsync(int? productId = null, bool includeRecalled = false)
        {
            var query = context.ProductBatches
                .Include(x => x.Product)
                .AsQueryable();

            if (productId.HasValue)
            {
                query = query.Where(x => x.ProductId == productId.Value);
            }

            if (!includeRecalled)
            {
                query = query.Where(x => !x.IsRecalled);
            }

            return await query
                .OrderBy(x => x.ExpiryDate)
                .ThenBy(x => x.BatchNumber)
                .ToListAsync();
        }

        public async Task<ProductBatch?> GetBatchByIdAsync(int id)
        {
            return await context.ProductBatches
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ProductBatch> CreateBatchAsync(ProductBatch batch, string? userName)
        {
            if (batch.QuantityReceived <= 0)
                throw new InvalidOperationException("Quantity received must be greater than zero.");

            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == batch.ProductId)
                ?? throw new KeyNotFoundException($"Product not found: {batch.ProductId}");

            var duplicateExists = await context.ProductBatches
                .AnyAsync(x => x.ProductId == batch.ProductId && x.BatchNumber == batch.BatchNumber);
            if (duplicateExists)
                throw new InvalidOperationException("A batch with the same batch number already exists for this product.");

            batch.QuantityRemaining = batch.QuantityReceived;
            context.ProductBatches.Add(batch);

            product.PreviousUnitsInStock = product.UnitsInStock;
            product.UnitsInStock += batch.QuantityReceived;

            AddStockReportEntry(product, "BatchIn", userName);

            await context.SaveChangesAsync();
            return batch;
        }

        public async Task<ProductBatch> RecallBatchAsync(int id, string reason, string? userName)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new InvalidOperationException("Recall reason is required.");

            var batch = await context.ProductBatches
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new KeyNotFoundException($"Batch not found: {id}");

            if (batch.IsRecalled)
                return batch;

            batch.IsRecalled = true;
            batch.RecalledOn = DateTime.UtcNow;
            batch.RecallReason = reason.Trim();

            if (batch.QuantityRemaining > 0)
            {
                var removedQty = batch.QuantityRemaining;
                batch.QuantityRemaining = 0;

                var product = batch.Product;
                product.PreviousUnitsInStock = product.UnitsInStock;
                product.UnitsInStock = Math.Max(0, product.UnitsInStock - removedQty);

                AddStockReportEntry(product, "Recall", userName);
            }

            await context.SaveChangesAsync();
            return batch;
        }

        public async Task<IEnumerable<ProductBatch>> GetExpiringBatchesAsync(int daysAhead = 30)
        {
            var cutoff = DateTime.UtcNow.Date.AddDays(daysAhead);

            return await context.ProductBatches
                .Include(x => x.Product)
                .Where(x => !x.IsRecalled && x.QuantityRemaining > 0 && x.ExpiryDate.Date <= cutoff)
                .OrderBy(x => x.ExpiryDate)
                .ThenBy(x => x.Product.Name)
                .ToListAsync();
        }

        public async Task<ProcedureProductUsage> RecordProcedureUsageAsync(ProcedureProductUsage usage, string? userName)
        {
            if (usage.QuantityUsed <= 0)
                throw new InvalidOperationException("Quantity used must be greater than zero.");

            var consultation = await context.AestheticConsultations
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == usage.ConsultationId)
                ?? throw new KeyNotFoundException($"Consultation not found: {usage.ConsultationId}");

            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == usage.ProductId)
                ?? throw new KeyNotFoundException($"Product not found: {usage.ProductId}");

            usage.ProcedureType = string.IsNullOrWhiteSpace(usage.ProcedureType)
                ? consultation.ProcedureType
                : usage.ProcedureType.Trim();

            var remainingToDeduct = usage.QuantityUsed;
            ProductBatch? selectedBatch = null;

            if (usage.ProductBatchId > 0)
            {
                selectedBatch = await context.ProductBatches
                    .FirstOrDefaultAsync(x => x.Id == usage.ProductBatchId && x.ProductId == usage.ProductId)
                    ?? throw new KeyNotFoundException($"Batch not found: {usage.ProductBatchId}");

                if (selectedBatch.IsRecalled)
                    throw new InvalidOperationException("Cannot use a recalled batch.");

                if (selectedBatch.ExpiryDate.Date < DateTime.UtcNow.Date)
                    throw new InvalidOperationException("Cannot use an expired batch.");

                if (selectedBatch.QuantityRemaining < remainingToDeduct)
                    throw new InvalidOperationException("Insufficient batch quantity.");

                selectedBatch.QuantityRemaining -= remainingToDeduct;
                remainingToDeduct = 0;
            }
            else
            {
                var availableBatches = await context.ProductBatches
                    .Where(x => x.ProductId == usage.ProductId
                                && !x.IsRecalled
                                && x.QuantityRemaining > 0
                                && x.ExpiryDate.Date >= DateTime.UtcNow.Date)
                    .OrderBy(x => x.ExpiryDate)
                    .ThenBy(x => x.Id)
                    .ToListAsync();

                foreach (var batch in availableBatches)
                {
                    if (remainingToDeduct <= 0)
                        break;

                    selectedBatch ??= batch;

                    var deduct = Math.Min(batch.QuantityRemaining, remainingToDeduct);
                    batch.QuantityRemaining -= deduct;
                    remainingToDeduct -= deduct;
                }

                if (remainingToDeduct > 0)
                    throw new InvalidOperationException("Insufficient stock across active non-expired batches.");
            }

            product.PreviousUnitsInStock = product.UnitsInStock;
            product.UnitsInStock = Math.Max(0, product.UnitsInStock - usage.QuantityUsed);

            usage.ProductBatchId = selectedBatch?.Id ?? usage.ProductBatchId;
            usage.ProductBatch = selectedBatch ?? await context.ProductBatches.FirstAsync(x => x.Id == usage.ProductBatchId);
            usage.UsedOn = usage.UsedOn == default ? DateTime.UtcNow : usage.UsedOn;

            context.ProcedureProductUsages.Add(usage);
            AddStockReportEntry(product, "ProcedureUse", userName);

            await context.SaveChangesAsync();
            return usage;
        }

        public async Task<IEnumerable<ProcedureProductUsage>> GetProcedureUsagesAsync(int? consultationId = null, int? productId = null)
        {
            var query = context.ProcedureProductUsages
                .Include(x => x.Product)
                .Include(x => x.ProductBatch)
                .Include(x => x.Consultation)
                .AsQueryable();

            if (consultationId.HasValue)
                query = query.Where(x => x.ConsultationId == consultationId.Value);

            if (productId.HasValue)
                query = query.Where(x => x.ProductId == productId.Value);

            return await query
                .OrderByDescending(x => x.UsedOn)
                .ThenByDescending(x => x.Id)
                .ToListAsync();
        }

        public async Task<int> UploadAsync(Stream fileStream, string fileName, int itemColumn, int qtyColumn, bool deleteExisting, string? userName, string? sheetName = null)
        {
            var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
            if (extension is not ".csv" and not ".xls" and not ".xlsx")
                throw new InvalidOperationException("Only .xls, .xlsx and .csv files are supported.");

            var itemColIndex = itemColumn > 0 ? itemColumn : 1;
            var qtyColIndex = qtyColumn > 0 ? qtyColumn : 3;

            var rows = extension == ".csv"
                ? ParseCsv(fileStream, itemColIndex, qtyColIndex)
                : ParseXlsx(fileStream, sheetName, itemColIndex, qtyColIndex);

            if (rows.Count == 0)
                return 0;

            var category = await context.ProductCategories
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync();

            if (category is null)
            {
                category = new ProductCategory
                {
                    Name = "General",
                    Description = "Auto-created default category"
                };
                context.ProductCategories.Add(category);
                await context.SaveChangesAsync();
            }

            if (deleteExisting)
            {
                context.ProcedureProductUsages.RemoveRange(context.ProcedureProductUsages);
                context.ProductBatches.RemoveRange(context.ProductBatches);
                context.ProductStockReports.RemoveRange(context.ProductStockReports);

                var productIds = await context.Products.AsNoTracking().Select(x => x.Id).ToListAsync();
                if (productIds.Count > 0)
                {
                    context.OrderDetails.RemoveRange(context.OrderDetails.Where(x => productIds.Contains(x.ProductId)));
                }

                context.Products.RemoveRange(context.Products);
                await context.SaveChangesAsync();
            }

            var inserted = 0;
            var uploadedProducts = new List<Product>();

            foreach (var row in rows)
            {
                var name = row.Name?.Trim();
                if (string.IsNullOrWhiteSpace(name))
                    continue;

                var product = new Product
                {
                    Name = name,
                    Description = null,
                    Icon = null,
                    BuyingPrice = 0,
                    PreviousBuyingPrices = 0,
                    PreviousSellingPrice = 0,
                    PreviousUnitsInStock = 0,
                    UnitsInStock = row.Quantity,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategoryId = category.Id,
                    ProductCategory = category
                };

                context.Products.Add(product);
                uploadedProducts.Add(product);
                inserted++;
            }

            if (inserted > 0)
            {
                await context.SaveChangesAsync();

                foreach (var product in uploadedProducts)
                {
                    if (product.UnitsInStock != 0)
                    {
                        AddStockReportEntry(product, "Upload", userName);
                    }
                }

                await context.SaveChangesAsync();
            }

            return inserted;
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

        private static List<UploadProductRow> ParseCsv(Stream stream, int itemColumn, int qtyColumn)
        {
            var rows = new List<UploadProductRow>();
            using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: true);

            var lineNumber = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                lineNumber++;

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var columns = line.Split(',');
                if (lineNumber == 1 && LooksLikeHeader(columns, itemColumn, qtyColumn))
                    continue;

                var item = columns.ElementAtOrDefault(itemColumn - 1)?.Trim();
                var qtyRaw = columns.ElementAtOrDefault(qtyColumn - 1)?.Trim();

                if (string.IsNullOrWhiteSpace(item))
                    continue;

                rows.Add(new UploadProductRow
                {
                    Name = item,
                    Quantity = ParseQuantity(qtyRaw)
                });
            }

            return rows;
        }

        private static List<UploadProductRow> ParseXlsx(Stream stream, string? sheetName, int itemColumn, int qtyColumn)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using var excelReader = ExcelReaderFactory.CreateReader(stream);
            var result = excelReader.AsDataSet(new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = false
                }
            });

            DataTable? table = null;
            if (!string.IsNullOrWhiteSpace(sheetName))
            {
                table = result.Tables.Cast<DataTable>().FirstOrDefault(x => x.TableName.Equals(sheetName, StringComparison.OrdinalIgnoreCase));
            }

            table ??= result.Tables.Count > 0 ? result.Tables[0] : null;
            if (table is null)
                return [];

            var startRow = table.Rows.Count > 0 && LooksLikeHeader(table.Rows[0], itemColumn, qtyColumn) ? 1 : 0;
            var rows = new List<UploadProductRow>();

            for (var i = startRow; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
                var item = row.ItemArray.ElementAtOrDefault(itemColumn - 1)?.ToString()?.Trim();
                var qtyRaw = row.ItemArray.ElementAtOrDefault(qtyColumn - 1)?.ToString()?.Trim();

                if (string.IsNullOrWhiteSpace(item))
                    continue;

                rows.Add(new UploadProductRow
                {
                    Name = item,
                    Quantity = ParseQuantity(qtyRaw)
                });
            }

            return rows;
        }

        private static bool LooksLikeHeader(string[] columns, int itemColumn, int qtyColumn)
        {
            var first = columns.ElementAtOrDefault(itemColumn - 1)?.Trim().ToLowerInvariant() ?? string.Empty;
            var second = columns.ElementAtOrDefault(qtyColumn - 1)?.Trim().ToLowerInvariant() ?? string.Empty;
            return first.Contains("item") || first.Contains("name") || first.Contains("product") || second.Contains("qty") || second.Contains("quantity") || second.Contains("stock");
        }

        private static bool LooksLikeHeader(DataRow row, int itemColumn, int qtyColumn)
        {
            var first = row.ItemArray.ElementAtOrDefault(itemColumn - 1)?.ToString()?.Trim().ToLowerInvariant() ?? string.Empty;
            var second = row.ItemArray.ElementAtOrDefault(qtyColumn - 1)?.ToString()?.Trim().ToLowerInvariant() ?? string.Empty;
            return first.Contains("item") || first.Contains("name") || first.Contains("product") || second.Contains("qty") || second.Contains("quantity") || second.Contains("stock");
        }

        private static int ParseQuantity(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return 0;

            var normalized = raw.Replace(",", string.Empty).Trim();
            if (int.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out var intValue))
                return Math.Max(0, intValue);

            if (decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out var decimalValue))
                return Math.Max(0, (int)Math.Round(decimalValue, MidpointRounding.AwayFromZero));

            return 0;
        }

        private sealed class UploadProductRow
        {
            public string? Name { get; set; }
            public int Quantity { get; set; }
        }
    }
}

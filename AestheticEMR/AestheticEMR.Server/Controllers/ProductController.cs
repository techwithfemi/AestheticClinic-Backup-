using AestheticEMR.Core.Models.Shop;
using AestheticEMR.Core.Services.Shop;
using AestheticEMR.Server.ViewModels.Shop;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AestheticEMR.Server.Controllers;

[Route("api/[controller]")]
[Authorize]
public class ProductController(ILogger<ProductController> logger, IMapper mapper, IProductService productService)
    : BaseApiController(logger, mapper)
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductVM>), 200)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var products = await productService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ProductVM>>(products));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products");
            AddModelError("Unable to retrieve products");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("stock-report")]
    [ProducesResponseType(typeof(IEnumerable<ProductStockReportVM>), 200)]
    public async Task<IActionResult> GetStockReport()
    {
        try
        {
            var report = await productService.GetStockReportAsync();
            return Ok(_mapper.Map<IEnumerable<ProductStockReportVM>>(report));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product stock report");
            AddModelError("Unable to retrieve product stock report");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductVM), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var product = await productService.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound(id);
            }

            return Ok(_mapper.Map<ProductVM>(product));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product {Id}", id);
            AddModelError("Unable to retrieve product");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("categories")]
    [ProducesResponseType(typeof(IEnumerable<ProductCategoryVM>), 200)]
    public async Task<IActionResult> GetCategories()
    {
        try
        {
            var categories = await productService.GetCategoriesAsync();
            return Ok(_mapper.Map<IEnumerable<ProductCategoryVM>>(categories));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product categories");
            AddModelError("Unable to retrieve product categories");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("categories/{id:int}")]
    [ProducesResponseType(typeof(ProductCategoryVM), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        try
        {
            var category = await productService.GetCategoryByIdAsync(id);
            if (category is null)
            {
                return NotFound(id);
            }

            return Ok(_mapper.Map<ProductCategoryVM>(category));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product category {Id}", id);
            AddModelError("Unable to retrieve product category");
            return BadRequest(ModelState);
        }
    }

    [HttpPost("categories")]
    [ProducesResponseType(typeof(ProductCategoryVM), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateCategory([FromBody] ProductCategoryEditVM model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var entity = _mapper.Map<ProductCategory>(model);
            var created = await productService.CreateCategoryAsync(entity);
            return CreatedAtAction(nameof(GetCategoryById), new { id = created.Id }, _mapper.Map<ProductCategoryVM>(created));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product category");
            AddModelError(ex.GetBaseException().Message);
            return BadRequest(ModelState);
        }
    }

    [HttpPut("categories/{id:int}")]
    [ProducesResponseType(typeof(ProductCategoryVM), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] ProductCategoryEditVM model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var existing = await productService.GetCategoryByIdAsync(id);
            if (existing is null)
            {
                return NotFound(id);
            }

            _mapper.Map(model, existing);
            existing.Id = id;

            var updated = await productService.UpdateCategoryAsync(existing);
            return Ok(_mapper.Map<ProductCategoryVM>(updated));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product category {Id}", id);
            AddModelError(ex.GetBaseException().Message);
            return BadRequest(ModelState);
        }
    }

    [HttpDelete("categories/{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            var existing = await productService.GetCategoryByIdAsync(id);
            if (existing is null)
            {
                return NotFound(id);
            }

            await productService.DeleteCategoryAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Delete blocked for product category {Id}", id);
            AddModelError(ex.Message);
            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product category {Id}", id);
            AddModelError("Unable to delete product category");
            return BadRequest(ModelState);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductVM), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] ProductEditVM model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var entity = _mapper.Map<Product>(model);
            var created = await productService.CreateAsync(entity, GetCurrentUserName());
            var loaded = await productService.GetByIdAsync(created.Id) ?? created;
            return CreatedAtAction(nameof(GetById), new { id = loaded.Id }, _mapper.Map<ProductVM>(loaded));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product");
            AddModelError(ex.GetBaseException().Message);
            return BadRequest(ModelState);
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ProductVM), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] ProductEditVM model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var existing = await productService.GetByIdAsync(id);
            if (existing is null)
            {
                return NotFound(id);
            }

            _mapper.Map(model, existing);
            existing.Id = id;

            await productService.UpdateAsync(existing, GetCurrentUserName());
            var loaded = await productService.GetByIdAsync(id) ?? existing;
            return Ok(_mapper.Map<ProductVM>(loaded));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product {Id}", id);
            AddModelError(ex.GetBaseException().Message);
            return BadRequest(ModelState);
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var existing = await productService.GetByIdAsync(id);
            if (existing is null)
            {
                return NotFound(id);
            }

            await productService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product {Id}", id);
            AddModelError("Unable to delete product");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("batches")]
    [ProducesResponseType(typeof(IEnumerable<ProductBatchVM>), 200)]
    public async Task<IActionResult> GetBatches([FromQuery] int? productId, [FromQuery] bool includeRecalled = false)
    {
        try
        {
            var batches = await productService.GetBatchesAsync(productId, includeRecalled);
            return Ok(_mapper.Map<IEnumerable<ProductBatchVM>>(batches));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product batches");
            AddModelError("Unable to retrieve product batches");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("batches/{id:int}")]
    [ProducesResponseType(typeof(ProductBatchVM), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetBatchById(int id)
    {
        try
        {
            var batch = await productService.GetBatchByIdAsync(id);
            if (batch is null)
                return NotFound(id);

            return Ok(_mapper.Map<ProductBatchVM>(batch));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product batch {Id}", id);
            AddModelError("Unable to retrieve product batch");
            return BadRequest(ModelState);
        }
    }

    [HttpPost("batches")]
    [ProducesResponseType(typeof(ProductBatchVM), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateBatch([FromBody] ProductBatchEditVM model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var entity = _mapper.Map<ProductBatch>(model);
            var created = await productService.CreateBatchAsync(entity, GetCurrentUserName());
            var loaded = await productService.GetBatchByIdAsync(created.Id) ?? created;
            return CreatedAtAction(nameof(GetBatchById), new { id = loaded.Id }, _mapper.Map<ProductBatchVM>(loaded));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product batch");
            AddModelError(ex.GetBaseException().Message);
            return BadRequest(ModelState);
        }
    }

    [HttpPut("batches/{id:int}/recall")]
    [ProducesResponseType(typeof(ProductBatchVM), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> RecallBatch(int id, [FromBody] RecallBatchVM model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var existing = await productService.GetBatchByIdAsync(id);
            if (existing is null)
                return NotFound(id);

            var recalled = await productService.RecallBatchAsync(id, model.Reason, GetCurrentUserName());
            var loaded = await productService.GetBatchByIdAsync(recalled.Id) ?? recalled;
            return Ok(_mapper.Map<ProductBatchVM>(loaded));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recalling batch {Id}", id);
            AddModelError(ex.GetBaseException().Message);
            return BadRequest(ModelState);
        }
    }

    [HttpGet("batches/expiring")]
    [ProducesResponseType(typeof(IEnumerable<ProductBatchVM>), 200)]
    public async Task<IActionResult> GetExpiringBatches([FromQuery] int daysAhead = 30)
    {
        try
        {
            var batches = await productService.GetExpiringBatchesAsync(daysAhead);
            return Ok(_mapper.Map<IEnumerable<ProductBatchVM>>(batches));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving expiring batches");
            AddModelError("Unable to retrieve expiring batches");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("procedure-usage")]
    [ProducesResponseType(typeof(IEnumerable<ProcedureProductUsageVM>), 200)]
    public async Task<IActionResult> GetProcedureUsage([FromQuery] int? consultationId, [FromQuery] int? productId)
    {
        try
        {
            var usages = await productService.GetProcedureUsagesAsync(consultationId, productId);
            return Ok(_mapper.Map<IEnumerable<ProcedureProductUsageVM>>(usages));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving procedure product usage");
            AddModelError("Unable to retrieve procedure product usage");
            return BadRequest(ModelState);
        }
    }

    [HttpPost("procedure-usage")]
    [ProducesResponseType(typeof(ProcedureProductUsageVM), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> RecordProcedureUsage([FromBody] ProcedureProductUsageEditVM model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var usage = _mapper.Map<ProcedureProductUsage>(model);
            var created = await productService.RecordProcedureUsageAsync(usage, GetCurrentUserName());
            var loaded = (await productService.GetProcedureUsagesAsync(consultationId: created.ConsultationId, productId: created.ProductId))
                .FirstOrDefault(x => x.Id == created.Id) ?? created;

            return StatusCode(201, _mapper.Map<ProcedureProductUsageVM>(loaded));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording procedure product usage");
            AddModelError(ex.GetBaseException().Message);
            return BadRequest(ModelState);
        }
    }

    [HttpPost("upload")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Upload(
        [FromForm] IFormFile file,
        [FromForm] bool deleteExisting = true,
        [FromForm] string? sheetName = null,
        [FromForm] int? itemColumn = null,
        [FromForm] int? qtyColumn = null)
    {
        if (file is null || file.Length == 0)
        {
            AddModelError("Please select a file to upload.");
            return BadRequest(ModelState);
        }

        try
        {
            await using var stream = file.OpenReadStream();
            var inserted = await productService.UploadAsync(
                stream,
                file.FileName,
                itemColumn ?? 1,
                qtyColumn ?? 3,
                deleteExisting,
                GetCurrentUserName(),
                sheetName);

            return Ok(new { inserted });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading products file");
            AddModelError(ex.GetBaseException().Message);
            return BadRequest(ModelState);
        }
    }

    private string? GetCurrentUserName()
    {
        return User.Identity?.Name
               ?? User.FindFirstValue("preferred_username")
               ?? User.FindFirstValue(ClaimTypes.Name)
               ?? GetCurrentUserId();
    }
}

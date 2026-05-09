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

    private string? GetCurrentUserName()
    {
        return User.Identity?.Name
               ?? User.FindFirstValue("preferred_username")
               ?? User.FindFirstValue(ClaimTypes.Name)
               ?? GetCurrentUserId();
    }
}

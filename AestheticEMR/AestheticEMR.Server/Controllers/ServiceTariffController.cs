using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using AestheticEMR.Server.ViewModels.Legacy;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Controllers;

[Route("api/[controller]")]
[Authorize]
public class ServiceTariffController(ILogger<ServiceTariffController> logger, IMapper mapper, IServiceTariffService serviceTariffService)
    : BaseApiController(logger, mapper)
{
    [HttpGet("companies")]
    [ProducesResponseType(typeof(IEnumerable<TariffCompanyVM>), 200)]
    public IActionResult GetCompanies()
    {
        try
        {
            var records = serviceTariffService.GetCompanies();
            return Ok(_mapper.Map<IEnumerable<TariffCompanyVM>>(records));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tariff companies");
            AddModelError("Unable to retrieve tariff companies");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("source-companies")]
    [ProducesResponseType(typeof(IEnumerable<TariffCompanyVM>), 200)]
    public IActionResult GetSourceCompanies([FromQuery] string? category = null)
    {
        try
        {
            var records = serviceTariffService.GetCompaniesWithTariffs(category);
            return Ok(_mapper.Map<IEnumerable<TariffCompanyVM>>(records));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving source tariff companies");
            AddModelError("Unable to retrieve source tariff companies");
            return BadRequest(ModelState);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ServiceTariffVM>), 200)]
    public async Task<IActionResult> GetAll([FromQuery] string? coyId, [FromQuery] string? search)
    {
        try
        {
            var records = await serviceTariffService.GetAllAsync(coyId, search);
            return Ok(_mapper.Map<IEnumerable<ServiceTariffVM>>(records));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving service tariffs");
            AddModelError("Unable to retrieve service tariffs");
            return BadRequest(ModelState);
        }
    }

    [HttpPost("upload")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Upload(
        [FromForm] IFormFile file,
        [FromForm] string coyId,
        [FromForm] bool deleteExisting = false,
        [FromForm] string? category = null,
        [FromForm] string? sheetName = null,
        [FromForm] int? itemColumn = null,
        [FromForm] int? qtyColumn = null)
    {
        if (file is null || file.Length == 0)
        {
            AddModelError("Please select a file to upload.");
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(coyId))
        {
            AddModelError("Company code is required.");
            return BadRequest(ModelState);
        }

        try
        {
            await using var stream = file.OpenReadStream();
            var inserted = await serviceTariffService.UploadAsync(coyId, stream, file.FileName, deleteExisting, category, sheetName, itemColumn, qtyColumn);
            return Ok(new { inserted });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading service tariff file for company {CoyId}", coyId);
            AddModelError(ex.GetBaseException().Message);
            return BadRequest(ModelState);
        }
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ServiceTariffVM), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            var record = await serviceTariffService.GetByIdAsync(id);
            if (record is null)
            {
                return NotFound(id);
            }

            return Ok(_mapper.Map<ServiceTariffVM>(record));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving service tariff {Id}", id);
            AddModelError("Unable to retrieve service tariff");
            return BadRequest(ModelState);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(ServiceTariffVM), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] ServiceTariffVM model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var entity = _mapper.Map<hServiceNHI>(model);
            entity.SNO = 0;

            if (string.IsNullOrWhiteSpace(entity.Company) && !string.IsNullOrWhiteSpace(model.CoyId))
            {
                entity.Company = model.CoyId;
            }

            var created = await serviceTariffService.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = created.SNO }, _mapper.Map<ServiceTariffVM>(created));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating service tariff");
            AddModelError(ex.Message);
            return BadRequest(ModelState);
        }
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ServiceTariffVM), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Update(long id, [FromBody] ServiceTariffVM model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var existing = await serviceTariffService.GetByIdAsync(id);
            if (existing is null)
            {
                return NotFound(id);
            }

            _mapper.Map(model, existing);
            existing.SNO = id;

            if (string.IsNullOrWhiteSpace(existing.Company) && !string.IsNullOrWhiteSpace(model.CoyId))
            {
                existing.Company = model.CoyId;
            }

            var updated = await serviceTariffService.UpdateAsync(existing);
            return Ok(_mapper.Map<ServiceTariffVM>(updated));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating service tariff {Id}", id);
            AddModelError(ex.GetBaseException().Message);
            return BadRequest(ModelState);
        }
    }

    [HttpPost("copy")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Copy([FromBody] CopyTariffRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.TargetCoyId) || string.IsNullOrWhiteSpace(request.SourceCoyId))
        {
            AddModelError("Both source and target company codes are required.");
            return BadRequest(ModelState);
        }

        try
        {
            var inserted = await serviceTariffService.CopyFromCompanyAsync(
                request.TargetCoyId, request.SourceCoyId, request.DeleteExisting, request.Category);
            return Ok(new { inserted });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error copying service tariff from {Source} to {Target}", request.SourceCoyId, request.TargetCoyId);
            AddModelError(ex.GetBaseException().Message);
            return BadRequest(ModelState);
        }
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var existing = await serviceTariffService.GetByIdAsync(id);
            if (existing is null)
            {
                return NotFound(id);
            }

            await serviceTariffService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting service tariff {Id}", id);
            AddModelError("Unable to delete service tariff");
            return BadRequest(ModelState);
        }
    }
}

public sealed record CopyTariffRequest(string TargetCoyId, string SourceCoyId, bool DeleteExisting, string? Category = null);

using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using AestheticEMR.Server.ViewModels.Legacy;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Controllers;

[Route("api/[controller]")]
[Authorize]
public class BillingController(
    ILogger<BillingController> logger,
    IMapper mapper,
    IBillingService billingService,
    IBillingCrossDatabaseSyncStrategyProvider billingSyncStrategyProvider)
    : BaseApiController(logger, mapper)
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BillingVM>), 200)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var invoices = await billingService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<BillingVM>>(invoices));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving invoices");
            AddModelError("Unable to retrieve invoices");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("{billNo}")]
    [ProducesResponseType(typeof(BillingVM), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByBillNo(string billNo)
    {
        try
        {
            var invoice = await billingService.GetByBillNoAsync(billNo);
            if (invoice is null)
            {
                return NotFound(billNo);
            }

            var details = await billingService.GetDetailsAsync(billNo);
            var vm = _mapper.Map<BillingVM>(invoice);
            vm.Details = _mapper.Map<List<BillingDetailVM>>(details);

            return Ok(vm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving invoice {BillNo}", billNo);
            AddModelError("Unable to retrieve invoice");
            return BadRequest(ModelState);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(BillingVM), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] BillingVM model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var billing = _mapper.Map<Billing>(model);
            var details = _mapper.Map<List<BillingDetail>>(model.Details);

            var result = await billingService.CreateAsync(billing, details);
            var response = _mapper.Map<BillingVM>(result.Billing);
            response.Details = _mapper.Map<List<BillingDetailVM>>(result.Details);

            return CreatedAtAction(nameof(GetByBillNo), new { billNo = response.BillNo }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating invoice");
            AddModelError(ex.GetBaseException().Message);
            return BadRequest(ModelState);
        }
    }

    [HttpPut("{billNo}")]
    [ProducesResponseType(typeof(BillingVM), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Update(string billNo, [FromBody] BillingVM model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var existing = await billingService.GetByBillNoAsync(billNo);
            if (existing is null)
            {
                return NotFound(billNo);
            }

            var billing = _mapper.Map<Billing>(model);
            var details = _mapper.Map<List<BillingDetail>>(model.Details);
            var result = await billingService.UpdateAsync(billNo, billing, details);

            var response = _mapper.Map<BillingVM>(result.Billing);
            response.Details = _mapper.Map<List<BillingDetailVM>>(result.Details);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating invoice {BillNo}", billNo);
            AddModelError(ex.GetBaseException().Message);
            return BadRequest(ModelState);
        }
    }

    [HttpDelete("{billNo}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(string billNo)
    {
        try
        {
            var existing = await billingService.GetByBillNoAsync(billNo);
            if (existing is null)
            {
                return NotFound(billNo);
            }

            await billingService.DeleteAsync(billNo);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting invoice {BillNo}", billNo);
            AddModelError("Unable to delete invoice");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("sync-status")]
    [ProducesResponseType(typeof(BillingSyncStatusVM), 200)]
    public IActionResult GetSyncStatus()
    {
        var status = billingSyncStrategyProvider.CurrentStatus;

        return Ok(new BillingSyncStatusVM
        {
            EffectiveMode = status.EffectiveMode,
            PrimaryDataSource = status.PrimaryDataSource,
            PrimaryDatabase = status.PrimaryDatabase,
            IncludedDatabases = status.IncludedDatabases.ToList(),
            SameInstanceDatabases = status.SameInstanceDatabases.ToList(),
            CrossInstanceDatabases = status.CrossInstanceDatabases.ToList(),
            Warnings = status.Warnings.ToList()
        });
    }
}

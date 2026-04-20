// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AestheticEMR.Core.Services.Legacy.Interfaces;
using AestheticEMR.Server.ViewModels.Legacy;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class HRetainershipController : BaseApiController
    {
        private readonly IHRetainershipService _retainershipService;

        public HRetainershipController(ILogger<HRetainershipController> logger, IMapper mapper,
            IHRetainershipService retainershipService) : base(logger, mapper)
        {
            _retainershipService = retainershipService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HRetainershipVM>), 200)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var retainerships = await _retainershipService.GetAllAsync();
                return Ok(_mapper.Map<IEnumerable<HRetainershipVM>>(retainerships));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving retainerships");
                AddModelError("Unable to retrieve retainerships");
                return BadRequest(ModelState);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(HRetainershipVM), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var retainership = await _retainershipService.GetByIdAsync(id);
                if (retainership == null)
                    return NotFound(id);

                return Ok(_mapper.Map<HRetainershipVM>(retainership));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving retainership {Id}", id);
                AddModelError("Unable to retrieve retainership");
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(HRetainershipVM), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] HRetainershipVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var retainership = _mapper.Map<AestheticEMR.Core.Models.Legacy.HRetainership>(model);
                var created = await _retainershipService.CreateAsync(retainership);
                return CreatedAtAction(nameof(GetById), new { id = created.RetainId }, _mapper.Map<HRetainershipVM>(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating retainership");
                AddModelError("Unable to create retainership");
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(HRetainershipVM), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(string id, [FromBody] HRetainershipVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existing = await _retainershipService.GetByIdAsync(id);
                if (existing == null)
                    return NotFound(id);

                _mapper.Map(model, existing);
                var updated = await _retainershipService.UpdateAsync(existing);
                return Ok(_mapper.Map<HRetainershipVM>(updated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating retainership {Id}", id);
                AddModelError("Unable to update retainership");
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var existing = await _retainershipService.GetByIdAsync(id);
                if (existing == null)
                    return NotFound(id);

                await _retainershipService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting retainership {Id}", id);
                AddModelError("Unable to delete retainership");
                return BadRequest(ModelState);
            }
        }
    }
}
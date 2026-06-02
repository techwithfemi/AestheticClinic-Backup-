using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Legacy;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace AestheticEMR.Server.Controllers
{
    [Route("api/product-tariff")]
    [Authorize]
    public class ProductTariffController : BaseApiController
    {
        private readonly ApplicationDbContext _db;
        public ProductTariffController(ApplicationDbContext db, ILogger<ProductTariffController> logger, IMapper mapper)
            : base(logger, mapper)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string coyID)
        {
            if (string.IsNullOrWhiteSpace(coyID))
                return BadRequest("Missing coyID");
            var tariffs = await _db.ProductTariffs.Where(x => x.Company == coyID).ToListAsync();
            return Ok(tariffs);
        }

        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(ProductTariff), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(long id, [FromBody] ProductTariff model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existing = await _db.ProductTariffs.FirstOrDefaultAsync(x => x.SNO == id);
                if (existing is null)
                {
                    return NotFound(id);
                }

                existing.PdtName = model.PdtName;
                existing.Category = model.Category;
                existing.Company = model.Company;
                existing.Price = model.Price;
                existing.Remarks = model.Remarks;
                existing.CoyName = model.CoyName;
                existing.Capitated = model.Capitated;
                existing.TariffStatus = model.TariffStatus;
                existing.RevType = model.RevType;
                existing.UsersCat = model.UsersCat;

                await _db.SaveChangesAsync();
                return Ok(existing);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error updating product tariff {Id}", id);
                AddModelError(ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }
    }
}

using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Legacy;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace AestheticEMR.Server.Controllers
{
    [Route("api/product-tariff")]
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
    }
}

using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Legacy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Server.Controllers
{
    [Route("api/drugnhis")]
    [ApiController]
    [Authorize]
    public class DrugNHISController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DrugNHISController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DrugNhi>>> GetDrugTariffs([FromQuery] string? coyID)
        {
            var query = _context.DrugNhis.AsNoTracking();
            if (!string.IsNullOrEmpty(coyID))
                query = query.Where(x => x.Company == coyID);
            var items = await query.ToListAsync();
            return Ok(items);
        }
    }
}

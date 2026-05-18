using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Legacy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Server.Controllers
{
    [Route("api/hservicenhis")]
    [ApiController]
    [Authorize]
    public class HServiceNHIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HServiceNHIController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<hServiceNHI>>> GetServiceTariffs([FromQuery] string? coyID)
        {
            var query = _context.hServiceNHIs.AsNoTracking();
            if (!string.IsNullOrEmpty(coyID))
                query = query.Where(x => x.Company == coyID);
            var items = await query.ToListAsync();
            return Ok(items);
        }
    }
}

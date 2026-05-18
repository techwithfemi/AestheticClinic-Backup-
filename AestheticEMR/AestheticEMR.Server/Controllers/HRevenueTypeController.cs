using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Legacy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Server.Controllers
{
    [Route("api/hrevenue")]
    [ApiController]
    [Authorize]
    public class HRevenueTypeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HRevenueTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<hRevenueType>>> GetRevenueTypes()
        {
            var items = await _context.Set<hRevenueType>().AsNoTracking().ToListAsync();
            return Ok(items);
        }
    }
}

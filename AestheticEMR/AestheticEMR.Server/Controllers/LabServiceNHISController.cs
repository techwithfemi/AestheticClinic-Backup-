using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Legacy;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AestheticEMR.Server.Controllers
{
    [Route("api/labservicenhis")]
    [ApiController]
    public class LabServiceNHISController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LabServiceNHISController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LabServiceNhi>>> GetLabServiceTariffs([FromQuery] string? coyID)
        {
            var query = _context.LabServiceNhis.AsNoTracking();
            if (!string.IsNullOrEmpty(coyID))
                query = query.Where(x => x.Company == coyID);
            var items = await query.ToListAsync();
            return Ok(items);
        }
    }
}

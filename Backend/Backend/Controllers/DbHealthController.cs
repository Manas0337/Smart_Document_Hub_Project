using Backend.Data;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/db-health")]
    public class DbHealthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DbHealthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Check()
        {
            var userCount = _context.Users.Count();
            return Ok(new
            {
                status = "Database connected",
                users = userCount
            });
        }
    }
}

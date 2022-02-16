using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers.AdminPanel
{
    [Route("api/admin/rcon/[action]")]
    [Authorize]
    [Authorize(Roles = "Owner")]
    public class RconController : Controller
    {
        private readonly ILogger<RconController> _logger;
        
        public RconController(ILogger<RconController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet()]
        public async Task<IActionResult> Status()
        {
            return Ok();
        }


    }
}

using System.Globalization;
using System.Threading.Tasks;
using Application;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PunishedController : Controller
    {
        private readonly PunishmentsStoreContext context;
        private readonly ILogger<PunishedController> _logger;

        public PunishedController(ILogger<PunishedController> logger, PunishmentsStoreContext punishmentsContext)
        {
            context = punishmentsContext;
            _logger = logger;
        }

        [HttpGet("{page}")]
        public async Task<IActionResult> BanList([FromServices] IServerService serversContext, uint id)
        {
            var servers = await serversContext.GetServersInfoAsync();

            var json = await context.GetBanList(servers, id);

            if (json == null)
            {
                return BadRequest("Не удалось получить список банов");
            }
            else
            {
                return Json(json);
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> SearchBans([FromServices] IServerService serversContext, string input, uint id)
        {
            if(input == null)
            {
                return BadRequest();
            }

            var servers = await serversContext.GetServersInfoAsync();

            var json = await context.GetSearchEntOfBans(servers, input, id);

            if (json == null)
            {
                return BadRequest("Не удалось найти список банов");
            }
            else
            {
                return Json(json);
            }
        }

        [HttpGet("{page}")]
        public async Task<IActionResult> CommsList([FromServices] IServerService serversContext, uint id)
        {
            var servers = await serversContext.GetServersInfoAsync();
            var json = await context.GetCommsList(servers, id);

            if (json == null)
            {
                return BadRequest("Не удалось получить список мутов");
            }
            else
            {
                return Json(json);
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> SearchComms([FromServices] IServerService serversContext, string input, uint id)
        {
            if(input == null)
            {
                return BadRequest();
            }

            var servers = await serversContext.GetServersInfoAsync();
            var json = await context.GetSearchEntOfComms(servers, input, id);

            if (json == null)
            {
                return BadRequest("Не удалось найти список мутов");
            }
            else
            {
                return Json(json);
            }
        }
    }
}

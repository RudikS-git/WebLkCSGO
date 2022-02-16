using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class ServerMonitoringController : Controller
    {
        private readonly IServerService _serverService;
        private readonly ILogger<ServerMonitoringController> _logger;

        public ServerMonitoringController(ILogger<ServerMonitoringController> logger, IServerService serverService)
        {
            _logger = logger;
            _serverService = serverService;

            _logger.LogDebug(1, "NLog injected into ServerMonitoringController");
        }

        [HttpGet()]
        [Produces("application/json")]
        public async Task<IActionResult> GetServersInfo([FromServices] IUserStatService iUserStatRepository)
        {
            _logger.LogInformation("GetServersInfo action");

            var serversInfo = await _serverService.GetServersInfoAsync();

            serversInfo.countNewPlayersDay = await iUserStatRepository.GetCountNewPlayersDayAsync();
            serversInfo.countPlayersDay = await iUserStatRepository.GetCountPlayersDayAsync();
            serversInfo.countPlayers = await iUserStatRepository.GetCountPlayersAsync();

            if (serversInfo != null)
            {
                return Json(serversInfo);
            }

            return BadRequest();
        }
    }
}

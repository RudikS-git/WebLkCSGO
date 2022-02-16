using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application;
using Application.DTO;
using Application.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Domain.Entities.Server;
using FluentValidation;
using WebAPI.Utils;
using WebAPI.Utils.ServiceToken;

namespace WebAPI.Controllers.AdminPanel
{
    [Route("api/admin/monitoring/[action]")]
    [Authorize]
    [Authorize(Roles = "Owner")]
    public class MonitoringAdminController : Controller
    {
        private IServerService _serverService;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly ILogger<MonitoringAdminController> _logger;
        private readonly IValidator<ServerDTO> _serverValidator;

        public MonitoringAdminController(ILogger<MonitoringAdminController> logger, 
                                         IServerService serverService, 
                                         ServiceResolver serviceResolver,
                                         IValidator<ServerDTO> serverValidator)
        {
            _serverService = serverService;
            _tokenBuilder = (ITokenBuilder)serviceResolver("S");
            _logger = logger;
            _serverValidator = serverValidator;
        }

        [HttpPost()]
        public async Task<IActionResult> AddServer(ServerDTO serverDto)
        {
            var result = await _serverValidator.ValidateAsync(serverDto);
            result.GetExceptionFromNotValide();

            Server server = new Server(serverDto.Ip, serverDto.Port);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, server.Ip + ":" + server.Port),
            };

            server.Token = _tokenBuilder.BuildToken(claims);
            
            if (await _serverService.AddServerAsync(server) != null)
            {
                return Ok("Сервер успешно добавлен!");
            }

            return BadRequest("Сервер с такими данными уже существует!");
        }

        [HttpGet()]
        public async Task<IActionResult> DeleteServer(int? id)
        {
            if (id == null || id == 0)
            {
                return BadRequest($"Неккоректный id = {id}");
            }

            if (await _serverService.DeleteServerAsync(id) != null)
            {
                return Ok("Сервер был успешно удалён");
            }
            
            return BadRequest("Сервер был успешно удалён");
        }

        [HttpPost()]
        public async Task<IActionResult> ChangeServerInfo(Server server)
        {
          //  var result = _serverValidator.ValidateAsync(server);
          //  result.
            
            if (server.Id != 0 || server.Ip == null || server.Port != 0)
            {
                return BadRequest("Некорректные входные данные");
            }

            if(await _serverService.EditServerAsync(server) != null)
            {
                return Ok("Сервер был успешно изменён");
            }

            return BadRequest("Не удалось изменить информацию о сервере");
        }

        [Authorize(AuthenticationSchemes = "User")]
        [HttpGet()]
        public async Task<IActionResult> GetServers()
        {
            var servers = await _serverService.GetServersAsync();

            return Json(servers);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application;
using Application.DTO;
using Application.DTO.Input;
using Application.Validators;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Domain.Entities.Support;
using FluentValidation;
using WebAPI.Signalar;

namespace WebAPI.Controllers.AdminPanel
{
    [Route("api/admin/tickets/[action]")]
    public class TicketsAdminController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IHubContext<AdminPanelHub> hubContext;
        private readonly ILogger<TicketsAdminController> _logger;
        private readonly IMapper _iMapper;
        private readonly IValidator<TicketEditDTO> _ticketEditValidator;

        public TicketsAdminController(ILogger<TicketsAdminController> logger,
                                      ITicketService ticketService,
                                      IHubContext<AdminPanelHub> adminHubContext,
                                      IMapper iMapper,
                                      IValidator<TicketEditDTO> ticketEditValidator)
        {
            _ticketService = ticketService;
            hubContext = adminHubContext;
            _logger = logger;
            _iMapper = iMapper;
            _ticketEditValidator = ticketEditValidator;
        }

        [Authorize]
        [Authorize(Policy = "TicketManage")]
        [HttpGet()]
        public async Task<IActionResult> GetById(int id)
        {
            var ticket = await _ticketService.GetAsync(id);

            if (ticket == null)
            {
                return BadRequest("Не удалось получить тикеты");
            }

            return Ok(ticket);
        }

        [Authorize]
        [Authorize(Policy = "TicketManage")]
        [HttpGet()]
        public async Task<IActionResult> Get(int page, int offset)
        {
            IEnumerable<TicketDTO> tickets = _ticketService.GetAsync(page, offset);

            if (tickets == null)
            {
                return BadRequest("Не удалось получить тикеты");
            }

            int count = await _ticketService.GetCount();
            
            return Ok(new { count, tickets });
        }

        [Authorize(Policy = "TicketManage")]
        [Produces("application/json")]
        [HttpPost()]
        public async Task<IActionResult> SetTicketState([FromBody]TicketEditDTO ticketEdit)
        {
            var result = await _ticketEditValidator.ValidateAsync(ticketEdit);
            result.GetExceptionFromNotValide();

            var ticketDTO = await _ticketService.SetTicketStateAsync(ticketEdit);

            if (ticketDTO == null)
            {
                return BadRequest("Не удалось изменить состояние тикета");
            }

            await hubContext.Clients.All.SendAsync("ticketStateUpdate");
            return Json(ticketDTO);
        }

        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Server")]
        [HttpPost()]
        public async Task<IActionResult> Add([FromBody] TicketAddDto ticket)
        {
            _logger.LogDebug($"{ticket.senderUserAuthId}, {ticket.accusedUserAuthId}, report message - {ticket.reportMessage}");
            
            var ipPort = User.Claims.Where(it => it.Type == ClaimTypes.NameIdentifier)
                                    .FirstOrDefault().Value;

            int id = await _ticketService.AddAsync(ipPort, ticket.senderUserAuthId, ticket.accusedUserAuthId, ticket.reportMessage);

            await hubContext.Clients.All.SendAsync("refreshTickets");

            return Json(new {id});
        }

        [Authorize]
        [Authorize(Policy = "TicketManage")]
        [HttpGet()]
        public async Task<IActionResult> GetTicketsByUser(int ticketId, string accusedUserStatId)
        {
            if (accusedUserStatId == null || ticketId <= 0)
            {
                return BadRequest("Некорректные входные данные");
            }

            var tickets = _ticketService.GetTicketsHistory(ticketId, accusedUserStatId);

            //var ticketAdminDto = _iMapper.Map<TicketAdminDTO>(tickets);
            //var ticketDTO = await _ticketService.SetTicketStateAsync(ticketAdminDto);

            if (tickets == null)
            {
                return BadRequest("Не удалось изменить состояние тикета");
            }

            return Json(tickets);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using SteamIDs_Engine;

namespace WebAPI.Controllers.AdminPanel
{
    [Route("api/admin/gamechat/[action]")]
    [Authorize]
    [Authorize(Policy = "GameChat")]
    public class GameChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IMapper _iMapper;

        public GameChatController(IChatService chatService, IMapper iMapper)
        {
            _chatService = chatService;
            _iMapper = iMapper;
        }

        [HttpGet]
        public IActionResult GetHistory(int serverId, int count, string steamId2)
        {
            var chatRows = _chatService.GetHistory(serverId, count, SteamIDConvert.Steam64ToSteam2(long.Parse(steamId2)));
            var gameChatDto = _iMapper.Map<IEnumerable<GameChatDTO>>(chatRows);

            return Ok(gameChatDto);
        }
    }
}
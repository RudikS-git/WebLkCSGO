using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PlayersController : Controller
    {
        private readonly IUserStatService _userStatService;
        private readonly IProfileService _profileService;

        public PlayersController(IUserStatService userStatService, IProfileService profileService)
        {
            _userStatService = userStatService;
            _profileService = profileService;
        }

        public IActionResult GetTopByPoints(int count)
        {
            var players = _userStatService.GetTopPlayersByPoints(count);

            if (players == null)
            {
                return BadRequest("Не удалось получить игроков");
            }

            return Json(players);
        }

        public IActionResult GetTopByHours(int count)
        {
            var players = _userStatService.GetTopPlayersByHours(count);

            if (players == null)
            {
                return BadRequest("Не удалось получить игроков");
            }

            return Json(players);
        }

        public async Task<IActionResult> GetUserStat(int id)
        {
            return Json(await _profileService.GetProfileAsync(id));
        }
    }
}

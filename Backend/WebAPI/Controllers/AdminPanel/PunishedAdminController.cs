using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.Controllers.AdminPanel;

namespace WebAPI.Controllers
{
    [Route("api/admin/punished/[action]")]
    [Authorize]
    [Authorize(Policy = "Unban")]
    public class PunishedAdminController : Controller
    {
        private readonly PunishmentsStoreContext _punishmentsStoreContext;
        private readonly ILogger<PunishedAdminController> _logger;

        public PunishedAdminController(ILogger<PunishedAdminController> logger, PunishmentsStoreContext punishmentsStoreContext)
        {
            _punishmentsStoreContext = punishmentsStoreContext;
            _logger = logger;
        }
        
        [HttpGet()]
        public async Task<IActionResult> UnbanUser(int? id, string uReason = null)
        {
            if(id == null)
            {
                return BadRequest("Некорректные входные данные");
            }

            if (await _punishmentsStoreContext.UnbanUser(id))
            {
                return Ok("Пользователь был успешно разбанен");
            }
            else
            {
                return BadRequest("При разбане пользователя произошла ошибка");
            }
        }

        [HttpGet()]
        public async Task<IActionResult> BanUser(int? id, string uReason = null)
        {
            if(id == null)
            {
                return BadRequest("Некорректные входные данные");
            }

            if (await _punishmentsStoreContext.BanUser(id))
            {
                return Ok("Пользователь был успешно заблокирован");
            }
            else
            {
                return BadRequest("Не удалось заблокировать пользователя");
            }
        }

        [HttpGet()]
        public async Task<IActionResult> EditBanCurrentInfo(uint id, string name, int typeBan, string steamid, string ip, int time, string reason)
        {
            bool isSuccess = await _punishmentsStoreContext.EditBanInfo(id, name, typeBan, steamid, ip, time, reason);

            if (isSuccess)
            {
                return Ok("Информация о блокировке пользователя была успешно изменена");
            }
            else
            {
                return BadRequest("Не удалось изменить информацию о блокировке пользователя");
            }
        }

        [HttpGet()]
        public async Task<IActionResult> EditCommsCurrentInfo(uint id, string name, int type, string steamid, int time, string reason)
        {
            bool isSuccess = await _punishmentsStoreContext.EditCommsInfo(id, name, type, steamid, time, reason);

            if (isSuccess)
            {
                return Ok("Информация о муте пользователя была успешно изменена");
            }
            else
            {
                return BadRequest("Не удалось изменить информацию о муте пользоателя");
            }
        }

        /*[HttpGet()]
        public string GetCulture()
        {
            HttpContext.Request.Headers["Accept-Language"].ToString();
            var str = HttpContext.Request.Headers["Accept-Language"][0].Split(new char[] { ',', ';' });
            // var ci = new CultureInfo(str[0]);


            return $"CurrentCulture:{str[0]}, CurrentUICulture:{str[0]}";
        }*/
    }
}

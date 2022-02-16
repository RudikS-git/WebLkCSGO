using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Domain.Entities.News;

namespace WebAPI.Controllers.AdminPanel
{
    [Route("api/admin/news/[action]")]
    [Authorize]
    [Authorize(Roles = "Owner")]
    public class NewsAdminController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ILogger<NewsAdminController> _logger;

        public NewsAdminController(ILogger<NewsAdminController> logger, INewsService newsService)
        {
            _newsService = newsService;
            _logger = logger;
        }

        [HttpPost()]
        public async Task<IActionResult> Add(News news)
        {
            if (news == null)
            {
                return BadRequest("Некорректные входные данные");
            }

            //if (await newsAdmin.AddAsync(news))
           // {
           //     return Ok("Новость успешно добавлена");
           // }

            return BadRequest("Не удалось добавить новость");
            
        }

        [HttpPost()]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest("Некорректные входные данные");
            }

            if (await _newsService.DeleteAsync(id))
            {
                return Ok("Новость успешно удалена");
            }

            return BadRequest("Не удалось удалить новость");
        }

        [HttpPost()]
        public async Task<IActionResult> Edit(News news)
        {
            if (news == null)
            {
                return BadRequest("Некорректные входные данные");
            }

            if (await _newsService.EditAsync(news))
            {
                return Ok("Новость успешно изменена");
            }

            return BadRequest("Не удалось изменить новость");
        }
    }
}

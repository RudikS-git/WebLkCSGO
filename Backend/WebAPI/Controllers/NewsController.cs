using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ILogger<NewsController> _logger;

        public NewsController(ILogger<NewsController> logger, INewsService newsService)
        {
            _newsService = newsService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet()]
        public IActionResult Page(int? id)
        {
            if(id == null)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.DTO.Input;
using Microsoft.Extensions.Configuration;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class VkController : Controller
    {
        private IVkService _vkService;

        public VkController(IVkService vkService)
        {
            _vkService = vkService;
        }

        [HttpPost]
        public IActionResult Callback([FromBody] VkUpdatesDto updates)
        {
          /*  switch (updates.Type)
            {
                // Если это уведомление для подтверждения адреса
                case "confirmation":
                    // Отправляем строку для подтверждения 
                    return Ok(_configuration["Config:Confirmation"]);
            }*/

            return Ok("ok");
        }
    }
}

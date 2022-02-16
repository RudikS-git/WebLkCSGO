using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.Logging;
using Domain.Entities.Account;
using Domain.Entities.News;
using Domain.Entities.Privilege;
using Domain.Entities.Product;

namespace WebAPI.Controllers.AdminPanel
{
    [Route("api/admin/privilege/[action]")]
    [Authorize]
    [Authorize(Roles = "Owner")]
    public class PrivilegeAdminController : Controller
    {
        private readonly ITypePrivilegeService _iTypePrivilegeService;
        private readonly IPrivilegeService _iPrivilegeService;
        private readonly ILogger<PrivilegeAdminController> _logger;

        public PrivilegeAdminController(ILogger<PrivilegeAdminController> logger, ITypePrivilegeService iTypePrivilegeService, IPrivilegeService iPrivilegeService)
        {
            _iTypePrivilegeService = iTypePrivilegeService;
            _logger = logger;
            _iPrivilegeService = iPrivilegeService;
        }

        [HttpPost()]
        public async Task<IActionResult> Add(TypePrivilege privilege)
        {
            if (privilege == null)
            {
                return BadRequest("Некорректные входные данные");
            }

            
            if (await _iTypePrivilegeService.AddAsync(privilege) != null)
            {
                 return Ok("Привилегия успешно добавлена");
            }

            return BadRequest("Не удалось добавить привилегию");
        }

        [HttpPost()]
        public async Task<IActionResult> EditTypePrivilege([FromBody] TypePrivilege privilege)
        {
            if (privilege == null)
            {
                return BadRequest("Некорректные входные данные");
            }

            if (await _iTypePrivilegeService.AddAsync(privilege) != null)
            {
                return Ok("Возможность привилегии успешно добавлена");
            }

            return BadRequest("Не удалось добавить привилегию");
        }
        
        [HttpPost()]
        public async Task<IActionResult> AddFeature(Feature feature)
        {
            if (feature == null && feature.Id > 0)
            {
                return BadRequest("Некорректные входные данные");
            }

            if (await _iTypePrivilegeService.AddFeatureAsync(feature) != null)
            {
                return Ok("Возможность привилегии успешно добавлена");
            }

            return BadRequest("Не удалось добавить возможность привилегии");
        }

        [HttpPost()]
        public async Task<IActionResult> EditFeature([FromBody] Feature feature)
        {
            if (feature == null && feature.Id > 0)
            {
                return BadRequest("Некорректные входные данные");
            }

            if (await _iTypePrivilegeService.EditFeatureAsync(feature) != null)
            {
                return Ok("Возможность привилегии успешно добавлена");
            }

            return BadRequest("Не удалось добавить возможность привилегии");
        }
    }
}

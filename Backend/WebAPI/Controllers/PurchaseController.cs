using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application;
using Application.DTO.Input;
using Application.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Qiwi.BillPayments.Model;
using SteamIDs_Engine;
using Domain.Entities.Account;
using Domain.Entities.Product;
using FluentValidation;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PurchaseController : Controller
    {
        private readonly IAccountService account;
        private readonly IPurchaseService _purchaseService;
        private readonly ITypePrivilegeService _typePrivilegeService;
        private readonly IPrivilegeService _privilegeService;

        private readonly IValidator<PurchaseDto> _purchaseValidator;

        private ILogger<PurchaseController> _logger;

        public PurchaseController(ILogger<PurchaseController> logger, 
                                  IAccountService accountRepository, 
                                  IPurchaseService purchaseService,
                                  ITypePrivilegeService typePrivilegeService,
                                  IPrivilegeService privilegeService,
                                  IValidator<PurchaseDto> purchaseValidator)
        {
            account = accountRepository;
            _purchaseService = purchaseService;
            _logger = logger;
            _typePrivilegeService = typePrivilegeService;
            _privilegeService = privilegeService;
            _purchaseValidator = purchaseValidator;
        }

        [HttpGet]
        public async Task<IActionResult> Privilege()
        {
            var steamId = HttpContext.User.Claims.FirstOrDefault(it => it.Type == ClaimTypes.NameIdentifier)?.Value;
            int steamId32 = 0;
            
            if (!string.IsNullOrEmpty(steamId))
            {
                steamId32 = SteamIDConvert.Steam64ToSteam32(long.Parse(steamId));
            }

            var privileges = await _typePrivilegeService.GetPrivilegesAsync(steamId32);

            if(privileges == null)
            {
                return BadRequest("Не удалось получить привилегии");
            }

            return Ok(privileges);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> Qiwi([FromBody] PurchaseDto purchaseDto)
        {
            var result = await _purchaseValidator.ValidateAsync(purchaseDto);
            result.GetExceptionFromNotValide();

            TypePrivilege priv = await _typePrivilegeService.GetTypePrivilegeAsync(purchaseDto.Id);

            if (priv == null)
            {
                return BadRequest("Данная привилегия не существует!");
            }

            if (await _privilegeService.IsCurrentPrivilegeBetter(SteamIDConvert.Steam64ToSteam32(long.Parse(purchaseDto.SteamId)), priv))
            {
                return BadRequest("Можно приобрести только привилегию выше по уровню!");
            }

            string url = await _purchaseService.PayAsync(purchaseDto.SteamId, priv, User.Identity.Name);

            if (string.IsNullOrEmpty(url))
            {
                return BadRequest("Не удалось сформировать оплату. Повторите попытку.");
            }

           // return new RedirectResult(url);
            return Ok(url);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> Notification([FromBody]Notification notification)
        {
            _logger.LogInformation($"{notification.Bill.BillId}, {notification.Bill.Amount}, {notification.Bill.Comment}");
            _logger.LogInformation($"{notification.Bill.CreationDateTime}, {notification.Bill.Status?.ValueString}");

            if (string.IsNullOrEmpty(notification.Bill.BillId))
            {
                return BadRequest("Некорректный billId");
            }

            var signature = HttpContext.Request.Headers
                                                    .FirstOrDefault(x => x.Key == "X-Api-Signature-SHA256")
                                                    .Value
                                                    .FirstOrDefault();

            if (string.IsNullOrEmpty(signature))
            {
                return BadRequest("Отсутствует заголовок сигнатуры");
            }
            
            await _purchaseService.CheckSuccessAsync(signature, notification, _privilegeService.Set);

            return Ok();
        }
    }
}

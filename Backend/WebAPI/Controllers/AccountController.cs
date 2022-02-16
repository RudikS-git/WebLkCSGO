using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

using Newtonsoft.Json;

using SteamIDs_Engine;

using System.Net;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using Application;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Infrastructure.SteamID;

using Infrastructure.SteamID.Models;
using Domain.Entities.Account;
using Infrastructure.Services.Cookie;
using Microsoft.AspNetCore.Http;
using WebAPI.Cookie;
using WebAPI.Utils;
using WebAPI.Utils.ServiceToken;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [Display(Name = "account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ITokenBuilder _iTokenBuilder;
        private readonly AuthCookie _authCookie;

        private readonly string domain;

        public AccountController(ILogger<AccountController> logger,
                                 IConfiguration configuration,
                                 IAccountService accountService,
                                 ServiceResolver serviceResolver,
                                 AuthCookie authCookie)
        {
            _accountService = accountService;
            _logger = logger;
            _configuration = configuration;
            _iTokenBuilder = (ITokenBuilder)serviceResolver("U");
            domain = _configuration.GetValue<string>("Domain");
            _authCookie = authCookie;
        }

        [HttpGet]
        public async Task ExternalLogin()
        {
            _logger.LogDebug("Заход в аутентификацию");
            
            var properties = new AuthenticationProperties() { RedirectUri = Url.Action("LoginCallBack")};
            properties.SetParameter("provider", "Steam");

           await  HttpContext.ChallengeAsync("Steam", properties);
          //  return new ChallengeResult("Steam", properties);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> LoginCallBack(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                throw new Exception($"Error from external provider: {remoteError}");
            }

          //  var result = await HttpContext.AuthenticateAsync("Steam");

            //if (result.Principal.Claims == null)
           // {
           //     return BadRequest("Не удалось авторизоваться");
           // }

            // есть ли пользователь в базе? Если есть берем данные его и вставляем в клэймы. Иначе создаем нового пользователя

            var auth = HttpContext.User.Claims.FirstOrDefault(it => it.Type == ClaimTypes.NameIdentifier).Value;
            var steamId = auth.Replace(@"https://steamcommunity.com/openid/id/", "");

            User user;
            if (!await _accountService.UserIsExist(steamId))
            {
                user = await _accountService.CreateUserAsync(steamId);
            }
            else
            {
                user = await _accountService.GetUser(steamId);
            }

            //result.Principal.FindFirstValue(ClaimTypes.Name)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, steamId),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("LastChanged", user.LastChanged.ToString())
            };

            //ClaimsIdentity id = new ClaimsIdentity(claims, "Steam", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            _authCookie.SetToken(HttpContext, _iTokenBuilder.BuildToken(claims));
            string refreshToken = _iTokenBuilder.GenerateRefreshToken();
            _authCookie.SetRefreshToken(HttpContext, refreshToken);

            await _accountService.SaveRefreshToken(steamId, refreshToken);
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

            if (string.IsNullOrEmpty(domain))
            {
                return RedirectToAction("auth/success");
            }
            else
            {
                return Redirect(domain + "/auth/success");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        { 
            HttpContext.Response.Cookies.Delete("refresh"); 
            HttpContext.Response.Cookies.Delete("token");

            if (string.IsNullOrEmpty(domain))
            {
                return RedirectToAction("");
            }
            else
            {
                return Redirect(domain);
            }
        }

        [Authorize(AuthenticationSchemes = "User")]
        [HttpGet]
        public async Task<IActionResult> UserInfo()
        {
            if(!User.Identity.IsAuthenticated)
            {
                /*if(!HttpContext.Request.Cookies.TryGetValue("token", out string token) || !HttpContext.Request.Cookies.TryGetValue("refresh", out string refresh))
                {
                    return Ok(new { isAuthenticated = User.Identity.IsAuthenticated } );
                }*/
                
                return Ok(new 
                {
                    isAuthenticated = User.Identity.IsAuthenticated
                });
            }
            else
            {
                var steamId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (steamId != null)
                {
                    var userInfo = await _accountService.GetUserInfo(steamId);

                    return Ok(userInfo);
                }
                else
                {
                    return Ok(new
                    {
                        isAuthenticated = false
                    });
                }
            }
        }

        [HttpGet]
        public IActionResult GetUserStat([FromServices] IUserStatService iUserStatRepository, string steamId)
        {
            return Json(iUserStatRepository.Get(steamId));
        }

        [HttpGet]
        public IActionResult GetPrivilege([FromServices] IPrivilegeService iPrivilegeService, int steamId)
        {
            return Json(iPrivilegeService.Get(steamId));
        }

        [HttpGet]
        public async Task<IActionResult> GetCountPlayersDay([FromServices] IUserStatService iUserStatRepository)
        {
            return Json(await iUserStatRepository.GetCountPlayersDayAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetCountNewPlayersDay([FromServices] IUserStatService iUserStatRepository)
        {
            var countNewPlayersDay = await iUserStatRepository.GetCountNewPlayersDayAsync();
            var countPlayersDay = await iUserStatRepository.GetCountPlayersDayAsync();

            return Json(new {
                countNewPlayersDay,
                countPlayersDay
            });
        }
    }
}

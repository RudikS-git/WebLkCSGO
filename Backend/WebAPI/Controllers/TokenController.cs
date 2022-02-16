using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application;
using Infrastructure.Services.Cookie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebAPI.Cookie;
using WebAPI.Utils;
using WebAPI.Utils.ServiceToken;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly ILogger<TokenController> _logger;
        private readonly AuthCookie _authCookie;

        public TokenController(IAccountService accountService,
                               ServiceResolver serviceResolver,
                               ILogger<TokenController> logger,
                               AuthCookie authCookie)
        {
            _accountService = accountService;
            _tokenBuilder = (ITokenBuilder)serviceResolver("U");
            _logger = logger;
            _authCookie = authCookie;
        }


        [HttpGet()] 
        //[Produces("application/json")] // [FromBody]TokenView tokenView
        public async Task<IActionResult> Refresh(string token)
        {
            if (!HttpContext.Request.Cookies.TryGetValue("refresh", out string refresh))
            {
                _logger.LogInformation("Отсутствует рефреш токен в куки");
                
                return BadRequest("Refresh token is lost");
            }
            
            var principal = _tokenBuilder.GetPrincipalFromExpiredToken(token);
            var authId = principal.Claims.FirstOrDefault(it => it.Type == ClaimTypes.NameIdentifier).Value; //this is mapped to the Name claim by default
            var user = await _accountService.GetUser(authId);

            var refreshToken = user.RefreshTokens.SingleOrDefault(it => it.Value == refresh);

            if (user == null || refreshToken == null || !refreshToken.IsActive)
            {
                HttpContext.Response.Cookies.Delete("token");
                HttpContext.Response.Cookies.Delete("refresh");

                _logger.LogInformation("Не удалось найти user, refreshToken или refreshToken срок завершился");

                return BadRequest("Invalid client request");
            }
            
            var newAccessToken = _tokenBuilder.BuildToken(principal.Claims);
            _authCookie.SetToken(HttpContext, newAccessToken);
            
            var newRefreshToken = _tokenBuilder.GenerateRefreshToken();
            await _accountService.SaveRefreshToken(authId, newRefreshToken);


            return Ok(new
            {
                accessToken = newAccessToken,
            });
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Revoke()
        {
            var authId = User.Claims.FirstOrDefault(it => it.Type == ClaimTypes.NameIdentifier).Value;
            var user = await _accountService.GetUser(authId);

            if (user == null) 
                return BadRequest();

            await _accountService.SaveRefreshToken(user.Auth64Id, null);
            
            return NoContent();
        }
    }
}

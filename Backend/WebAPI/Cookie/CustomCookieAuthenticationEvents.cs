using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using Infrastructure.Services;
using Infrastructure.Services.Cookie;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using WebAPI.Utils;
using WebAPI.Utils.ServiceToken;

namespace WebAPI.Cookie
{
    public class CustomCookieAuthenticationEvents : JwtBearerEvents
    {
        private readonly IAccountService _accountService;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly AuthCookie _authCookie;


        public CustomCookieAuthenticationEvents(IAccountService accountService, ServiceResolver serviceResolver, AuthCookie authCookie)
        {
            _accountService = accountService;
            _tokenBuilder = (ITokenBuilder)serviceResolver("U");
            _authCookie = authCookie;
        }

        public override async Task TokenValidated(TokenValidatedContext context)
        {
            var userPrincipal = context.Principal;
            var auth = userPrincipal.Claims
                            .Where(it => it.Type == ClaimTypes.NameIdentifier)
                            .FirstOrDefault()
                            .Value;

            if (!string.IsNullOrEmpty(auth) || auth != null)
            {
                var lastChanged = (from c in userPrincipal.Claims
                    where c.Type == "LastChanged"
                    select c.Value).FirstOrDefault();

                if (string.IsNullOrEmpty(lastChanged) || !(await _accountService.ValidateLastChanged(auth, lastChanged)))
                {
                    var user = await _accountService.GetUser(auth);

                    if (user == null)
                        return;
                    //context.RejectPrincipal(); //заставляет рассматривать запрос как анонимный. Нам не нужно т.к изменение наших данных в бд
                    // нужно лишь для изменения ролей и другой информации.

                    var role = user.Role?.Name;
                    var currentLastChanged = user.LastChanged.ToString();

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Auth64Id),
                        new Claim(ClaimTypes.Role, role),
                        new Claim("LastChanged", currentLastChanged)
                    };

                    _authCookie.SetToken(context.HttpContext, _tokenBuilder.BuildToken(claims));

                    if (context.HttpContext.Request.Cookies.TryGetValue("refresh", out string valueRefreshToken))
                    {
                        await _accountService.RejectToken(user.Auth64Id, valueRefreshToken);
                    }
                    
                    string refreshToken = _tokenBuilder.GenerateRefreshToken();

                    _authCookie.SetRefreshToken(context.HttpContext, refreshToken);
                    
                    await _accountService.SaveRefreshToken(user.Auth64Id, refreshToken);
                    
                    // ClaimsIdentity id = new ClaimsIdentity(claims, "Steam", ClaimsIdentity.DefaultNameClaimType,
                    //ClaimsIdentity.DefaultRoleClaimType);

                    //context.ReplacePrincipal(new ClaimsPrincipal(id));
                    // context.ShouldRenew = true;

                    // костыли
                    //await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    //await context.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
                } 
            }
        }
    }
}

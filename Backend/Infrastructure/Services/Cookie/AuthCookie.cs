using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Cookie
{
    public class AuthCookie
    {
        public AuthCookieOptions _authCookieOptions;

        public AuthCookie(AuthCookieOptions authCookieOptions)
        {
            _authCookieOptions = authCookieOptions;
        }
        
        public void SetToken(HttpContext context, string token)
        {
            context.Response.Cookies.Append("token", token, new CookieOptions()
            {
                Domain = _authCookieOptions.Domain,
                SameSite = SameSiteMode.Strict,
                Secure = true,
                HttpOnly = false,
                Expires = DateTime.Now.AddDays(30) // ставим срок как у refresh,
                                                   // т.к иначе они удалятся из браузера и
                                                   // создание нового будет невозможно без старого токена
            });
        }

        public void SetRefreshToken(HttpContext context, string refreshToken)
        {
            context.Response.Cookies.Append("refresh", refreshToken, new CookieOptions()
            {
                Domain = _authCookieOptions.Domain,
                SameSite = SameSiteMode.Strict,
                Secure = true,
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(30),
            });
        }
    }
}

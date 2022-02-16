using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Cookie;
using System.Configuration;

namespace WebAPI.Extensions
{
    public static class JwtTokenConfig
    {
        public static IServiceCollection AddJwtTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "User";
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, cfg =>
                {
                    cfg.ForwardDefaultSelector = httpContext => "User";
                })
                .AddCookie()
                /*  .AddCookie()/*options =>
                  {
                      options.Cookie.MaxAge = TimeSpan.FromDays(30);
                      options.ExpireTimeSpan = DateTime.Now.AddDays(30).TimeOfDay;
                      options.Cookie.Name = "caid";

                      // options.Cookie.Domain = ".dreams-server.xyz";
                      //
                      options.EventsType = typeof(CustomCookieAuthenticationEvents);

                  })*/
                // https://stackoverflow.com/questions/52952600/multiple-jwt-authorities-issuers-in-asp-net-core
                .AddJwtBearer("User", cfg =>
                {
                    cfg.RequireHttpsMetadata = true;
                    cfg.SaveToken = true;

                    cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration.GetValue<string>("AuthUserApiSecretKey"))),
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.FromSeconds(600)
                    };

                    cfg.EventsType = typeof(CustomCookieAuthenticationEvents);
                })
                .AddJwtBearer("Server", cfg =>
                {
                    cfg.RequireHttpsMetadata = true;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration.GetValue<string>("ServersApiSecretKey"))),
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = false,
                        RequireExpirationTime = false,
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuerSigningKey = true
                    };

                })
                .AddSteam(options =>
                {
                    options.ApplicationKey = configuration.GetValue<string>("SteamApiKey");
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                });
            
            return services;
        }
    }
}

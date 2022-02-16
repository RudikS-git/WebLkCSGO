using System;
using System.Collections.Generic;
using System.Text;
using Application;
using Application.Interfaces;
using Infrastructure.Services;
using Infrastructure.Services.Cookie;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.AddTransient<INewsService, NewsService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IServerService, ServerAdmin>();
            services.AddTransient<IUserStatService, UserStatService>();
            services.AddTransient<ITypePrivilegeService, TypePrivilegeService>();
            services.AddTransient<ITicketService, TicketService>();
            services.AddTransient<IPurchaseService, PurchaseService>();
            services.AddTransient<IVkService, VkService>();
            services.AddTransient<IPrivilegeService, PrivilegeService>();
            services.AddTransient<IChatService, ChatService>();
            services.AddTransient<IProfileService, ProfileService>();
        }

        public static void AddAuthCookie(this IServiceCollection services, IConfiguration config, IWebHostEnvironment environment)
        {
            AuthCookieOptions authCookie;
            
            if (!environment.IsDevelopment())
            {
                authCookie = new AuthCookieOptions()
                {
                    Domain = $".{config.GetValue<String>("ClearDomain")}"
                };
            }
            else
            {
                authCookie = new AuthCookieOptions()
                {
                    Domain = "localhost"
                };
            }

            services.AddTransient<AuthCookie>(options => new AuthCookie(authCookie));
        }
    }
}

using System;

using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using AspNet.Security.OpenId.Steam;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Application.Mappings;
using AspNet.Security.OpenId;
using AutoMapper;
using AutoWrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;
using WebAPI.Cookie;
using WebAPI.Middlewares;

using WebAPI.Signalar;
using WebAPI.Utils.ServiceToken;
using Domain.Context;
using Domain.Contexts;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Utils;
using WebAPI.Extensions;

namespace Infrastructure
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "My API", Version = "v1"}); });

            services.AddAutoMapper(options => options.AddProfile(new MappingProfile()));

            services.AddSignalR(options => options.HandshakeTimeout = TimeSpan.FromSeconds(30));

            services.AddCors();

            services.AddControllersWithViews()
                    .AddNewtonsoftJson(options => { options.SerializerSettings.DateFormatString = "dd.MM.yyyy HH:mm"; });

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = @"../ClientApp/build"; });

            services.AddJwtTokenAuthentication(Configuration);
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Unban", policy =>
                {
                    policy.RequireRole("Owner", "Trusted");
                });

                options.AddPolicy("TicketManage", policy =>
                {
                    policy.RequireRole("Owner", "Trusted", "Admin");
                });

                options.AddPolicy("GameChat", policy =>
                {
                    policy.RequireRole("Owner", "Trusted", "Admin");
                });
            });

            services.AddScoped<CustomCookieAuthenticationEvents>();


            // https://stackoverflow.com/questions/39174989/how-to-register-multiple-implementations-of-the-same-interface-in-asp-net-core
            services.AddScoped<ServerTokenBuilder>();
            services.AddScoped<UserTokenBuilder>();
            services.AddScoped<ServiceResolver>(serviceProvider => key =>
            {
                switch(key)
                {
                    case "S":
                        return serviceProvider.GetService<ServerTokenBuilder>();

                    case "U":
                        return serviceProvider.GetService<UserTokenBuilder>();

                    default:
                        throw new KeyNotFoundException();
                }
            });

            services.AddDbContext<DataContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(Configuration.GetConnectionString("DefaultConnection")))
                                             .EnableDetailedErrors();
            });

            services.Add(new ServiceDescriptor(typeof(PunishmentsStoreContext),
                            new PunishmentsStoreContext(Configuration.GetConnectionString("SourceBans"))));

            services.AddHttpClient<SteamService>(); // keep all clients in services
            services.AddSharedInfrastructure(Configuration); // connect all our services
            services.AddValidationServices(Configuration); // connect validations

            services.AddAuthCookie(Configuration, Environment);

            services.AddSingleton<IVkApi>(sp =>
            {
                var api = new VkApi();
                api.Authorize(new ApiAuthParams {AccessToken = Configuration["Vk:AccessToken"]});
                return api;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
          //  app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<CookieToHeaderHandlerMiddleware>();
            app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions()
            {
                IsApiOnly = false,
                ExcludePaths = new AutoWrapperExcludePath[]
                {
                    new AutoWrapperExcludePath("/signalServer/.*|/signalServer", ExcludeMode.Regex)
                },
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            // ����������� ����� useRouting � useAuthentication
            app.UseCors(builder =>
            {
                builder
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(Configuration.GetValue<string>("Domain"));
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                endpoints.MapHub<AdminPanelHub>("/signalServer");
            });

            if (env.IsDevelopment())
            {
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = @"../../ClientApp";
                   // spa.UseProxyToSpaDevelopmentServer("https://localhost:8081");
                    spa.UseReactDevelopmentServer(npmScript: "start");

                });
            }

            var supportedCultures = new[]
            {
                new CultureInfo("ru-RU"),
                new CultureInfo("ru"),
                new CultureInfo("en-US"),
                new CultureInfo("en")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("ru-RU"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Test API V1");
            });
    }
    }
}

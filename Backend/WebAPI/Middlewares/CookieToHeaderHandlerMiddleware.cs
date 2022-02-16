using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace WebAPI.Middlewares
{
    // https://www.cyberforum.ru/asp-net-mvc/thread2573778.html
    public class CookieToHeaderHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<CookieToHeaderHandlerMiddleware> _logger;

        public CookieToHeaderHandlerMiddleware(RequestDelegate next, ILogger<CookieToHeaderHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out StringValues stringValues))
            {
                if (context.Request.Cookies.TryGetValue("token", out string value))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + value);
                    _logger.LogInformation($"Заголовок Authorization установлен!");
                }
                else
                {
                    _logger.LogInformation("Не удалось получить token из куки. Заголовок не установлен!");
                }
            }
            else
            {
                _logger.LogInformation("Заголовок уже был установлен!");
            }

            await _next(context).ConfigureAwait(false);
        }
    }
}

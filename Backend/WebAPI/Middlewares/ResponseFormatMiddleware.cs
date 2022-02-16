using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebAPI.Wrappers;

namespace WebAPI.Middlewares
{
    static class StringExtension
    {
        public static bool IsValidJson(this string text)
        {
            text = text.Trim();
            if ((text.StartsWith("{") && text.EndsWith("}")) || //For object
                (text.StartsWith("[") && text.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(text);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    public class ResponseFormatMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseFormatMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Response.HasStarted)
            {
                return;
            }

            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;
            
            await _next(context);

            if (context.Response.StatusCode == (int) HttpStatusCode.OK)
            {
                var body = await FormatResponse(context.Response);
                await HandleSuccessRequestAsync(context, body, context.Response.StatusCode);
            }


            //     byte[] bytes = new byte[100];
        }

        private static Task HandleSuccessRequestAsync(HttpContext context, object body, int code)
        {
            context.Response.ContentType = "application/json";
            string jsonString, bodyText = string.Empty;
            Response apiResponse = null;


            if (!body.ToString().IsValidJson())
                bodyText = JsonConvert.SerializeObject(body);
            else
                bodyText = body.ToString();

            dynamic bodyContent = JsonConvert.DeserializeObject<dynamic>(bodyText);
            Type type;

            type = bodyContent?.GetType();

            if (type.Equals(typeof(Newtonsoft.Json.Linq.JObject)))
            {
                //apiResponse = JsonConvert.DeserializeObject<Response>(bodyText);

                apiResponse = new Response(bodyContent);
                jsonString = JsonConvert.SerializeObject(apiResponse);
                
            }
            else
            {
                apiResponse = new Response(bodyContent);
                jsonString = JsonConvert.SerializeObject(apiResponse);
            }

            return context.Response.WriteAsync(jsonString);
        }
        
        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var plainBodyText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return plainBodyText;
        }

       
        
    }
}

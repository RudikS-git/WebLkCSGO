using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Infrastructure.Exceptions;
using WebAPI.Wrappers;

namespace WebAPI.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!IsExclude(context))
            {
                await _next(context);
            }
            else
            {
                try
                {
                    var response = context.Response;
                    response.ContentType = "application/json";
                    var responseModel = new Response(null) { Data = context.Response.Body, IsError = false };
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    var result = JsonConvert.SerializeObject(responseModel);

                    await response.WriteAsync(result);

                    await _next(context);
                }
                catch (Exception error)
                {
                    var response = context.Response;
                    response.ContentType = "application/json";
                    var responseModel = new Response(null) { IsError = true, Message = error?.Message };

                    switch (error)
                    {
                        case ServiceException e:
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;

                        case ApplicationException e:
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;

                        case Infrastructure.Exceptions.ValidationException e:
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            responseModel.Errors = e.Errors;
                            break;

                        case KeyNotFoundException e:
                            response.StatusCode = (int)HttpStatusCode.NotFound;
                            break;

                        default:
                            response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }

                    var result = JsonConvert.SerializeObject(responseModel);

                    await response.WriteAsync(result);
                }
            }
        }

        public bool IsExclude(HttpContext context)
        {
            if (   !context.Request.Path.Value.Contains("signal")
                && !context.Request.Path.Value.Contains(".js")
                && !context.Request.Path.Value.Contains(".css")
                && !context.Request.Path.Value.Contains(".html"))
                return true;

            return false;
        }
    }
}

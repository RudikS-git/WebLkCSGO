using System;
using System.Collections.Generic;

namespace WebAPI.Wrappers
{
    public class Response
    {
        public string ApiVersion { get; set; }
        
        public bool IsError { get; set; }

        public string Message { get; set; }
        
        public List<string> Errors { get; set; }
        
        public object Data { get; set; }

        public Response(object data, string message = null, string apiVersion = "1.0")
        {
            IsError = true;
            Message = message;
            Data = data;
        }

        public Response(object data, string apiVersion = "1.0")
        {
            IsError = true;
            Data = data;
        }

        public Response(string message, string apiVersion = "1.0")
        {
            IsError = false;
            Message = message;
        }
    }
}

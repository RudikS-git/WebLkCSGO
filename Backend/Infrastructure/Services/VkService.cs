using System;
using Application;
using Microsoft.Extensions.Configuration;
using VkNet.Abstractions;
using VkNet.Model.RequestParams;

namespace Infrastructure.Services
{
    public class VkService : IVkService
    {
        private IVkApi _vkApi;
        private IConfiguration _configuration;

        public VkService(IVkApi vkApi, IConfiguration configuration)
        {
            _vkApi = vkApi;
            _configuration = configuration;
        }

        public void SendMessage(string message)
        {
            _vkApi.Messages.Send(new MessagesSendParams()
            {
                RandomId = new DateTime().Millisecond,
                Message = message,
                PeerId = 2000000003,
            });
        }
    }
}

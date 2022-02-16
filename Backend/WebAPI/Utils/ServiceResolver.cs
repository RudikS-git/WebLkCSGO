using System;

namespace WebAPI.Utils
{
    public interface IService { };

    public delegate IService ServiceResolver(string key);
}

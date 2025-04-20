// utils/Config.cs
using Microsoft.Extensions.Configuration;

namespace Canaa.AppHost.utils
{
    public static class Config
    {
        private static IConfiguration _configuration;

        public static void Init(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string GetConnectionString(string name = "DefaultConnection")
        {
            return _configuration.GetConnectionString(name);
        }
    }
}

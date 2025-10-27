using Microsoft.Extensions.Configuration;

namespace Canaa.Infra.Entities
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


        public static IConfigurationRoot Build()
        {
            if (_configuration != null)
            {
                // Converte IConfiguration em IConfigurationRoot para compatibilidade
                return new ConfigurationBuilder()
                    .AddConfiguration(_configuration)
                    .Build();
            }

            // Fallback caso _configuration não esteja inicializado
            return new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }

        public static string GetConnectionString()
        {
            return Build().GetConnectionString("DefaultConnection");
        }
    }
}

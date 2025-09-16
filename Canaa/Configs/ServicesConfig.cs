using Canaa.AppHost.Models;
using Microsoft.EntityFrameworkCore;

namespace Canaa.Configs
{
    public static class ServicesConfig
    {
        public static void Configure(IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString,
                ServerVersion.AutoDetect(connectionString))
            );

            services.AddControllers();
            services.AddEndpointsApiExplorer();
        }

    }
}

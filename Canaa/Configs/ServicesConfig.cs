using Canaa.AppHost.Models;
using Microsoft.EntityFrameworkCore;

namespace Canaa.Configs
{
    public static class ServicesConfig
    {
        public static void Configure(IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(config.GetConnectionString("DefaultConnection"),
                ServerVersion.AutoDetect(config.GetConnectionString("DefaultConnection"))));

            services.AddControllers();
            services.AddEndpointsApiExplorer();
        }
    }
}

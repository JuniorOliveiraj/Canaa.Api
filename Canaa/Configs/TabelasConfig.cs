using Canaa.Infra.Entities.BefDb;
using Canaa.Infra.Entities.Context;
using Canaa.Infra.Entities.Entities;
using Canaa.Infra.Entities.Tabelasbef;

namespace Canaa.Configs
{
    public static class TabelasConfig
    {
        public static void Configure(IServiceCollection services, IConfiguration config)
        {
            // Registra seu repositório e outros serviços
            services.AddScoped<IRepository<Z_USUARIO>, Repository<Z_USUARIO>>();

        }
    }
}

using Canaa.Infra.Entities.BefDb;
using Canaa.Infra.Entities.Context;
using Microsoft.Extensions.Configuration;
using Ninject;
using Ninject.Web.Common;

public class NinjectBindingsInfraEntitis
{
    public static void Register(IKernel kernel)
    {
        // Binding genérico: cobre TODAS as entidades
        kernel.Bind(typeof(IRepository<>))
              .To(typeof(Repository<>))
              .InRequestScope();

        // Contexto
        kernel.Bind<ApplicationDbContext>()
              .ToSelf()
              .InRequestScope();

        // Configuração (você pode remover o duplicado)
        kernel.Bind<IConfiguration>().ToMethod(ctx =>
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return configBuilder.Build();
        }).InSingletonScope();
    }
}

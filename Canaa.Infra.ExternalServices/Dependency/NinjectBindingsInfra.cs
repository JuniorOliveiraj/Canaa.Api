using Canaa.DataContracts.Auth.Context;
using Canaa.Infra.ExternalServices.Context;
using Canaa.Infra.ExternalServices.Query.QueryFunciotosQ;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Ninject;

namespace Canaa.Infra.ExternalServices.Dependency
{
    public class NinjectBindingsInfra
    {
        public static void Register(IKernel kernel)
        {
            kernel.Bind<ITodasFuncoesQuery>().To<TodasFuncoesQuery>();
            kernel.Bind<IUserContext>().To<UserContext>(); // Adicione isso aqui
            kernel.Bind<IHttpContextAccessor>().To<HttpContextAccessor>();
            kernel.Bind<IConfiguration>().ToMethod(ctx =>
            {
                var configBuilder = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // garante que o appsettings.json será encontrado
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                return configBuilder.Build();
            }).InSingletonScope();


            kernel.Bind<IConfiguration>().ToMethod(ctx =>
            {
                return new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
            }).InSingletonScope();

        }
    }
}

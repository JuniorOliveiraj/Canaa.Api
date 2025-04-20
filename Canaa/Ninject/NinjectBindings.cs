
using Ninject;
using Canaa.FN.BusinessComponents.Buscar.Gastos;
using Canaa.FN.BusinessComponents.Auth;

namespace Canaa.Ninject
{
    public class NinjectBindings
    {
        public static void Register(IKernel kernel)
        {
            // Aqui você faz todos os seus bindings
            // Exemplo:
            // kernel.Bind<IMeuServico>().To<MeuServico>();
            kernel.Bind<ILogin>().To<Login>();
            kernel.Bind<ITotalGastos>().To<TotalGastos>();
            kernel.Bind<IUsuarioLogadoMetodo>().To<UsuarioLogadoMetodo>();
            kernel.Bind<IConfiguration>().ToMethod(ctx =>
            {
                var configBuilder = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                return configBuilder.Build();
            }).InSingletonScope(); 


        }
    }
}

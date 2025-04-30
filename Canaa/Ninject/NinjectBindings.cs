
using Ninject;
using Canaa.FN.BusinessComponents.Buscar.Gastos;
using Canaa.FN.BusinessComponents.Auth;
using Canaa.FN.BusinessComponents.Adicionar.AdicionarGastos;
using Canaa.FN.BusinessComponents.Video.CriarVideos;
using Canaa.FN.BusinessComponents.Video.Youtube;


namespace Canaa.Ninject
{
    public class NinjectBindings
    {
        public static void Register(IKernel kernel)
        {
            // Aqui você faz todos os seus bindings
            // Exemplo:
            // kernel.Bind<IMeuServico>().To<MeuServico>();
           
            //Auth
            kernel.Bind<ICriarConta>().To<CriarConta>();
            kernel.Bind<ILogin>().To<Login>();

            //Gastos
            kernel.Bind<ITotalGastos>().To<TotalGastos>();
            kernel.Bind<IUsuarioLogadoMetodo>().To<UsuarioLogadoMetodo>();
            kernel.Bind<IAdicionarJsonGastosMercadoPago>().To<AdicionarJsonGastosMercadoPago>();

            //WF
            kernel.Bind<IVideosVertical>().To<VideosVertical>();

            kernel.Bind<IYoutubeComponent>().To<YoutubeComponent>();

            //Configuração
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

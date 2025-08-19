
using Canaa.FN.BusinessComponents.Adicionar.AdicionarGastos;
using Canaa.FN.BusinessComponents.Auth;
using Canaa.FN.BusinessComponents.Financas.Buscar.Gastos;
using Canaa.FN.BusinessComponents.Midia.Audio;
using Canaa.FN.BusinessComponents.Midia.Video.CriarVideos;
using Canaa.FN.BusinessComponents.Midia.Video.Youtube;
using Canaa.FN.BusinessComponents.Response;
using Canaa.FN.BusinessComponents.Usuarios;
using Ninject;


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
            kernel.Bind<IAuthContaUsuario>().To<AuthContaUsuario>();
            kernel.Bind<IUsuariosBusiness>().To<UsuariosBusiness>();

            //Gastos
            kernel.Bind<ITotalGastos>().To<TotalGastos>();
            kernel.Bind<IUsuarioLogadoMetodo>().To<UsuarioLogadoMetodo>();
            kernel.Bind<IAdicionarJsonGastosMercadoPago>().To<AdicionarJsonGastosMercadoPago>();

            //WF
            kernel.Bind<IVideosVertical>().To<VideosVertical>();
            kernel.Bind<IYoutubeComponent>().To<YoutubeComponent>();
            kernel.Bind<IControleTarefas>().To<ControleTarefas>();
            kernel.Bind<IGerarAudioComTts>().To<GerarAudioComTts>();

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

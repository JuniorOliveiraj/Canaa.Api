

//using Projects.CanaaComum.Gastos;

using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Canaa.AppHost.Ninject
{
    public static class DependencyInjection
    {
        public static void RegisterServices(IKernel kernel)
        {
            // Registrar os componentes de negócio
         // kernel.Bind<IClass>().To<Class>();

            // Registrar o formulário principal
          // kernel.Bind<PaineilPrincipal>().ToSelf();
        }
    }
}

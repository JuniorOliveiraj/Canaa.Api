using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Canaa.AppHost.utils
{
    public abstract class BusinessComponent
    {
        private static IKernel _kernel;

        // Método para registrar o Kernel (chamado uma vez na inicialização)
        public static void Initialize(IKernel kernel)
        {
            _kernel = kernel;
            
        }

        // Criar instância usando o Kernel do Ninject
        public static T CreateInstance<T>() where T : class
        {
            if (_kernel == null)
                throw new InvalidOperationException("O Kernel do Ninject não foi inicializado.");

            return _kernel.Get<T>();
        }
    }
}

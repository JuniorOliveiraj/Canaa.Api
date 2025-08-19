using Canaa.Infra.Entities.BefDb;
using Canaa.Infra.Entities.Context;
using Canaa.Infra.Entities.Entities;
using Microsoft.Extensions.Configuration;
using Ninject;
using Ninject.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.Infra.Entities.Dependecy
{
    public class NinjectBindingsInfraEntitis
    {
        public static void Register(IKernel kernel)
        {
            kernel.Bind<IRepository<Z_USUARIO>>()
                 .To<Repository<Z_USUARIO>>()
                 .InRequestScope();

            kernel.Bind<ApplicationDbContext>()
              .ToSelf()
              .InRequestScope();


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

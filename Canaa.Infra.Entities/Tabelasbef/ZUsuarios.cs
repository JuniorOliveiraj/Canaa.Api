using Canaa.Infra.Entities.BefDb;
using Canaa.Infra.Entities.Dependecy;
using Canaa.Infra.Entities.Entities;
using Canaa.Infra.Entities.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.Infra.Entities.Tabelasbef
{
    public static class ZUsuarios
    {
        public static Task<Z_USUARIO?> GetFirstOrDefault(params Criteria[] criterias)
        {
            // Resolve o repository do seu kernel Ninject
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Z_USUARIO>>();

            // E delega a busca dinâmica
            return repo.GetFirstOrDefaultAsync(criterias);
        }
    }


}

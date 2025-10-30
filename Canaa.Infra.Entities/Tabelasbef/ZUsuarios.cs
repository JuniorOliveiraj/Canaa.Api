using Canaa.Infra.Entities.BefDb;
using Canaa.Infra.Entities.Dependecy;
using Canaa.Infra.Entities.Utils;
using System.Collections.Generic;

namespace Canaa.Infra.Entities.Entities
{
    public static class ZUsuarios
    {
        public static Z_USUARIO? GetFirstOrDefault(params Criteria[] criterias)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Z_USUARIO>>();
            // Espera o resultado da Task de forma síncrona
            return repo.GetFirstOrDefaultAsync(criterias).GetAwaiter().GetResult();
        }

        public static List<Z_USUARIO> GetAll(params Criteria[] criterias)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Z_USUARIO>>();
            return repo.GetAllAsync().GetAwaiter().GetResult();
        }

        public static List<Z_USUARIO> GetMany(params Criteria[] criterias)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Z_USUARIO>>();
            return repo.GetMany(criterias).GetAwaiter().GetResult();
        }

        public static Z_USUARIO? GetForEdit(params Criteria[] criterias)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Z_USUARIO>>();
            return repo.GetForEditAsync(criterias).GetAwaiter().GetResult();
        }

        public static void Save(Z_USUARIO usuario)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Z_USUARIO>>();
            repo.UpdateAsync(usuario).GetAwaiter().GetResult();
        }
    }
}

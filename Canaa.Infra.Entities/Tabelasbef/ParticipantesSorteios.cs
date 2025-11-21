using Canaa.Infra.Entities.BefDb;
using Canaa.Infra.Entities.Dependecy;
using Canaa.Infra.Entities.Entities;
using Canaa.Infra.Entities.Utils;
using Canaa.TempModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.Infra.Entities.Tabelasbef
{
    public static class ParticipantesSorteios
    {
        public static Participants? GetFirstOrDefault(params Criteria[] criterias)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Participants>>();
            // Espera o resultado da Task de forma síncrona
            return repo.GetFirstOrDefaultAsync(criterias).GetAwaiter().GetResult();
        }

        public static List<Participants> GetAll(params Criteria[] criterias)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Participants>>();
            return repo.GetAllAsync().GetAwaiter().GetResult();
        }

        public static List<Participants> GetMany(params Criteria[] criterias)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Participants>>();
            return repo.GetMany(criterias).GetAwaiter().GetResult();
        }

        public static Participants? GetForEdit(params Criteria[] criterias)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Participants>>();
            return repo.GetForEditAsync(criterias).GetAwaiter().GetResult();
        }

        public static void Save(Participants usuario)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Participants>>();
            repo.UpdateAsync(usuario).GetAwaiter().GetResult();
        }

        public static void Create(Participants participante)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Participants>>();
            repo.AddAsync(participante).GetAwaiter().GetResult();
        }


        public static void DeleteMany(params Criteria[] criterias)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Participants>>();
            repo.DeleteManyAsync(criterias).GetAwaiter().GetResult();
        }
        public static void DeleteAll()
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Participants>>();
            repo.DeleteAllAsync().GetAwaiter().GetResult();
        }



    }
}

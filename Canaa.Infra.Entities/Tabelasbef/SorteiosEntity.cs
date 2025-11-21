using Canaa.Infra.Entities.BefDb;
using Canaa.Infra.Entities.Dependecy;
using Canaa.Infra.Entities.Utils;
using Canaa.TempModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.Infra.Entities.Tabelasbef
{
    public static class SorteiosEntity
    {
        public static Sorteios? GetFirstOrDefault(params Criteria[] criterias)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Sorteios>>();
            // Espera o resultado da Task de forma síncrona
            return repo.GetFirstOrDefaultAsync(criterias).GetAwaiter().GetResult();
        }

        public static List<Sorteios> GetAll(params Criteria[] criterias)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Sorteios>>();
            return repo.GetAllAsync().GetAwaiter().GetResult();
        }

        public static List<Sorteios> GetMany(params Criteria[] criterias)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Sorteios>>();
            return repo.GetMany(criterias).GetAwaiter().GetResult();
        }

        public static Sorteios? GetForEdit(params Criteria[] criterias)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Sorteios>>();
            return repo.GetForEditAsync(criterias).GetAwaiter().GetResult();
        }

        public static void Save(Sorteios usuario)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Sorteios>>();
            repo.UpdateAsync(usuario).GetAwaiter().GetResult();
        }

        public static void Create(Sorteios participante)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Sorteios>>();
            repo.AddAsync(participante).GetAwaiter().GetResult();
        }
 


        public static void DeleteMany(params Criteria[] criterias)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Sorteios>>();
            repo.DeleteManyAsync(criterias).GetAwaiter().GetResult();
        }
        public static void DeleteAll()
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<Sorteios>>();
            repo.DeleteAllAsync().GetAwaiter().GetResult();
        }



    }
}

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
    public static class ZContatosEmail 
    {
        public static ContatosEmail? GetFirstOrDefault(params Criteria[] criterias)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<ContatosEmail>>();
            return repo.GetFirstOrDefaultAsync(criterias).GetAwaiter().GetResult();
        }

        public static List<ContatosEmail> GetAll(params Criteria[] criterias)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<ContatosEmail>>();
            return repo.GetAllAsync().GetAwaiter().GetResult();
        }

        public static List<ContatosEmail> GetMany(params Criteria[] criterias)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<ContatosEmail>>();
            return repo.GetMany(criterias).GetAwaiter().GetResult();
        }
        public static ContatosEmail? GetForEdit(params Criteria[] criterias)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<ContatosEmail>>();
            return repo.GetForEditAsync(criterias).GetAwaiter().GetResult();
        }

        public static void Save(ContatosEmail usuario)
        {
            var repo = BusinessComponentInfraEntities.CreateInstance<IRepository<ContatosEmail>>();
            repo.UpdateAsync(usuario).GetAwaiter().GetResult();
        }
    }
    public enum ZContatosEmailStatusEmailComercial
    {
        Invalido,
        Inativo,
    }
    public enum ZContatosEmailStatusEmailPrincipal
    {
        Invalido,
        Inativo,
    }
}

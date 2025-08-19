using Canaa.Infra.Entities.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.Infra.Entities.BefDb
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetFirstOrDefaultAsync(params Criteria[] criterias);
    }
}

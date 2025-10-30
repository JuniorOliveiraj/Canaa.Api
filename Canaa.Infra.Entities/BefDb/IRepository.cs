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
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetMany(params Criteria[] criterias);
        Task<T?> GetFirstOrDefaultAsync(params Criteria[] criterias);
        Task<T?> GetForEditAsync(params Criteria[] criterias);
        Task UpdateAsync(T entity);

    }
}

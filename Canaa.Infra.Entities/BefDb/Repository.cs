using Canaa.Infra.Entities.Context;
using Canaa.Infra.Entities.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Canaa.Infra.Entities.BefDb
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _ctx;
        public Repository(ApplicationDbContext ctx) => _ctx = ctx;

        public async Task<T?> GetFirstOrDefaultAsync(params Criteria[] criterias)
        {
            IQueryable<T> q = _ctx.Set<T>();
            foreach (var c in criterias)
            {
                // montando a expressão de filtro...
                var param = Expression.Parameter(typeof(T), "x");
                var prop = Expression.Property(param, c.PropertyName);
                var constant = Expression.Constant(c.Value);
                var eq = Expression.Equal(prop, constant);
                var lambda = Expression.Lambda<Func<T, bool>>(eq, param);
                q = q.Where(lambda);
            }
            return await q.FirstOrDefaultAsync();
        }
    }
}

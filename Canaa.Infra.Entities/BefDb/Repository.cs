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

        public Task<List<T>> GetAllAsync()
        {
            return _ctx.Set<T>().ToListAsync();
        }

        public async Task<T?> GetFirstOrDefaultAsync(params Criteria[] criterias)
        {
            IQueryable<T> q = ApplyCriterias(_ctx.Set<T>(), criterias);
            return await q.AsNoTracking().FirstOrDefaultAsync(); // modo leitura
        }

        public async Task<T?> GetForEditAsync(params Criteria[] criterias)
        {
            IQueryable<T> q = ApplyCriterias(_ctx.Set<T>(), criterias);
            return await q.FirstOrDefaultAsync(); // modo edição (rastreado)
        }

        public Task<List<T>> GetMany(params Criteria[] criterias)
        {
            IQueryable<T> q = ApplyCriterias(_ctx.Set<T>(), criterias);
            return q.AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            _ctx.Set<T>().Add(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _ctx.Set<T>().Update(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _ctx.Set<T>().Remove(entity);
            await _ctx.SaveChangesAsync();
        }

        // 🔥 Aqui acontece a mágica
        private IQueryable<T> ApplyCriterias(IQueryable<T> query, Criteria[] criterias)
        {
            foreach (var c in criterias)
            {
                var param = Expression.Parameter(typeof(T), "x");
                var prop = Expression.Property(param, c.PropertyName);

                var propType = Nullable.GetUnderlyingType(prop.Type) ?? prop.Type;
                var convertedValue = Convert.ChangeType(c.Value, propType);
                var constant = Expression.Constant(convertedValue, prop.Type);

                Expression? body = c.Operator switch
                {
                    ComparisonOperator.Equals => Expression.Equal(prop, constant),
                    ComparisonOperator.NotEquals => Expression.NotEqual(prop, constant),
                    ComparisonOperator.GreaterThan => Expression.GreaterThan(prop, constant),
                    ComparisonOperator.LessThan => Expression.LessThan(prop, constant),
                    ComparisonOperator.Contains => BuildStringMethod(prop, constant, nameof(string.Contains)),
                    ComparisonOperator.StartsWith => BuildStringMethod(prop, constant, nameof(string.StartsWith)),
                    _ => throw new NotSupportedException($"Operador {c.Operator} não suportado")
                };

                var lambda = Expression.Lambda<Func<T, bool>>(body!, param);
                query = query.Where(lambda);
            }

            return query;
        }

        private static Expression BuildStringMethod(MemberExpression prop, ConstantExpression constant, string methodName)
        {
            if (prop.Type != typeof(string))
                throw new InvalidOperationException($"Operador '{methodName}' só pode ser usado em propriedades string.");

            return Expression.Call(prop, typeof(string).GetMethod(methodName, new[] { typeof(string) })!, constant);
        }
    }
}

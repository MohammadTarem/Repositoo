using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;

namespace Repositoo
{
    public class SqlOperations<Id, TEntity> : IDatabaseOperations<Id, TEntity>
      where TEntity : class

    {
        
        protected Func<TEntity, Id> getId;

        protected DbContext Context { get; private set; }

        public SqlOperations(DbContext context, Expression<Func<TEntity, Id>> expression)
        {
            Context = context;
            getId = expression.Compile();
        }

        public async Task<Id> AddAsync(TEntity item, CancellationToken cancllationToken = default)
        {
            await Context.Set<TEntity>().AddAsync(item, cancllationToken);
            var result = await Context.SaveChangesAsync(cancllationToken);
            return result > 0 ? getId(item) : default;
        }

        public async Task<Id> UpdateAsync(TEntity item, CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                Context.Set<TEntity>().Attach(item);

            }, cancellationToken);
            
            var result = await Context.SaveChangesAsync(cancellationToken);
            return result > 0 ? getId(item) : default;

        }

        public async Task<Id> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                Context.Set<TEntity>().Remove(entity);

            }, cancellationToken);

            
            return await Context.SaveChangesAsync(cancellationToken) > 0 ? getId(entity) : default;
        }

        public void Dispose() => Context.Dispose();

        public Type ElementType => Context.Set<TEntity>().AsQueryable().GetType();

        public Expression Expression => Context.Set<TEntity>().AsQueryable().Expression;

        public IQueryProvider Provider => Context.Set<TEntity>().AsQueryable().Provider;

        public IEnumerator<TEntity> GetEnumerator() => Context.Set<TEntity>().AsQueryable().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Context.Set<TEntity>().AsQueryable().GetEnumerator();

        //protected Expression<Func<TEntity, bool>> MakeSearchExpression(Expression<Func<TEntity, Id>> expression, Id key)
        //{
        //    var fieldName = (expression.Body as MemberExpression).Member.Name;
        //    var parameter = Expression.Parameter(typeof(TEntity));
        //    var constant = Expression.Constant(key);


        //    var memberAccess = Expression.PropertyOrField(parameter, fieldName);
        //    var equalityExpression = Expression.Equal(memberAccess, constant);

        //    return Expression.Lambda<Func<TEntity, bool>>(equalityExpression, new ParameterExpression[] { parameter });

        //}

    }
}

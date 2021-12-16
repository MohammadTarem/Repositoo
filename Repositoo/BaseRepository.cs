using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Repositoo
{
    public abstract class BaseRepository<Id, TEntity> : IQueryable<TEntity>, IDisposable
        where TEntity : class
    {

        protected IDatabaseOperations<Id, TEntity> operations;

        

        public BaseRepository(IDatabaseOperations<Id, TEntity> operations)
        {
            this.operations = operations;
            
        }

        public Id Add(TEntity item) => operations.AddAsync(item).Result;

        public Id Update(TEntity item) => operations.UpdateAsync(item).Result;

        public Id Delete(TEntity key) => operations.DeleteAsync(key).Result;

        public virtual Task<Id> AddAsync(TEntity item, CancellationToken cancellationToken = default) => operations.AddAsync(item, cancellationToken);

        public virtual Task<Id> UpdateAsync(TEntity item, CancellationToken cancellationToken = default) => operations.UpdateAsync(item, cancellationToken);

        public virtual Task<Id> DeleteAsync(TEntity key, CancellationToken cancellationToken = default) => operations.DeleteAsync(key, cancellationToken);

        public void Dispose() => operations.Dispose();

        public Type ElementType => operations.ElementType;

        public Expression Expression => operations.Expression;

        public IQueryProvider Provider => operations.Provider;

        public IEnumerator<TEntity> GetEnumerator() => operations.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => operations.GetEnumerator();
    }

    
}

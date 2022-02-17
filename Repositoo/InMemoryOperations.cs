using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Repositoo
{
    public class InMemoryOperations<TId, TEntity> : IDatabaseOperations<TId, TEntity>
    {
        protected List<TEntity> entities;
        protected Func<TEntity, TId> getId;

        public InMemoryOperations(Expression< Func<TEntity, TId>> getKey)
        {
            entities = new List<TEntity>();
            getId = getKey.Compile();
        }

        public Type ElementType => entities.AsQueryable().GetType();

        public Expression Expression => entities.AsQueryable().Expression;

        public IQueryProvider Provider => entities.AsQueryable().Provider;

        public IEnumerator<TEntity> GetEnumerator() => entities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => entities.GetEnumerator();

        protected Task<int> FindIndexAsync(TId id, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {

                for (int i = 0; i < entities.Count; ++i)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (id.Equals(getId(entities[i])))
                    {
                        return i;
                    }

                }
                return -1;
            });
        }

        public Task<TId> AddAsync(TEntity item, CancellationToken cancellationToken = default)
        {
            
            return Task.Factory.StartNew( () =>
            {

                cancellationToken.ThrowIfCancellationRequested();
                entities.Add(item);
                return getId(item);

            }, cancellationToken);
        }

        public Task<TId> UpdateAsync(TEntity item, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew<TId>(() =>
            {

                var id = getId(item);
                var index = FindIndexAsync(id, cancellationToken).Result;
                if(index == -1)
                {
                    return default;
                }
                cancellationToken.ThrowIfCancellationRequested();
                entities[index] = item;
                return id;


            }, cancellationToken);
        }

        public Task<TId> DeleteAsync(TEntity item, CancellationToken cancellationToken = default)
        {

            if(item == null)
            {
                return default;
            }

            return Task.Factory.StartNew(() =>
            {

                var id = getId(item);
                var index = FindIndexAsync(id, cancellationToken).Result;
                if (index == -1)
                {
                    return default;
                }
                cancellationToken.ThrowIfCancellationRequested();
                entities.RemoveAt(index);
                return id;


            }, cancellationToken);

        }

        public void Dispose()
        {
            entities.Clear();
        }

        
    }
}

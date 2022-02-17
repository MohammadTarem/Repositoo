using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Repositoo
{
    public interface IDatabaseOperations<TId, TEntity> : IQueryable<TEntity>, IDisposable
    {

        public Task<TId> AddAsync(TEntity item, CancellationToken cancellationToken = default);
        public Task<TId> UpdateAsync(TEntity item, CancellationToken cancellationToken = default);
        public Task<TId> DeleteAsync(TEntity item, CancellationToken cancellationToken = default);

    }

}

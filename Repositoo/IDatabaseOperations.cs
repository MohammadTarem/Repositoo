using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Repositoo
{
    public interface IDatabaseOperations<Id, TEntity> : IQueryable<TEntity>, IDisposable
    {

        public Task<Id> AddAsync(TEntity item, CancellationToken cancllationToken = default);
        public Task<Id> UpdateAsync(TEntity item, CancellationToken cancellationToken = default);
        public Task<Id> DeleteAsync(TEntity key, CancellationToken cancellationToken = default);

    }

}

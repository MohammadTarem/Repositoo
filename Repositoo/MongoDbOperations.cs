using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;



namespace Repositoo
{
    public class MongoDbOperations<Id, TEntity> : IDatabaseOperations<Id, TEntity>
        where TEntity : class
    {
        protected IMongoCollection<TEntity> collection;
        protected Expression<Func<TEntity, Id>> getKey;
        

        public MongoDbOperations(IMongoClient client, string name, Expression<Func<TEntity, Id>> key)
        {
            
            collection = client.GetDatabase(name).GetCollection<TEntity>(name);
            getKey = key;
        }

        public async Task<Id> AddAsync(TEntity item, CancellationToken cancellationToken = default)
        {
            await collection.InsertOneAsync(item, new InsertOneOptions {  BypassDocumentValidation = false}, cancellationToken);
            return GetKey(item);
        }

        public async Task<Id> UpdateAsync(TEntity item, CancellationToken cancellationToken = default)
        {
            var filter = Builders<TEntity>.Filter.Eq(getKey, GetKey(item));
            var result = await collection.ReplaceOneAsync(filter, item, new ReplaceOptions { IsUpsert = true }, cancellationToken);
            return result.ModifiedCount > 0 ? GetKey(item) : default;
        }

        public async Task<Id> DeleteAsync(TEntity item, CancellationToken cancellationToken)
        {
            var filter = Builders<TEntity>.Filter.Eq(getKey, GetKey(item));
            var result = await collection.FindOneAndDeleteAsync(filter, null, cancellationToken);
            return result != null ? GetKey(item) : default;

        }

        public Id GetKey(TEntity item) => getKey.Compile()(item);

        public Type ElementType => collection.AsQueryable().ElementType;

        public Expression Expression => collection.AsQueryable().Expression;

        public IQueryProvider Provider => collection.AsQueryable().Provider;

        IEnumerator IEnumerable.GetEnumerator() => collection.AsQueryable().GetEnumerator();

        public IEnumerator<TEntity> GetEnumerator() => collection.AsQueryable().GetEnumerator();

        public void Dispose() => collection = null;


    }
}

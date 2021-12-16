
// Spin a mongo docker image
// docker run --rm -e AUTH=no -p 27017:27017 mongo


using Repositoo;
namespace RepositooSample
{
    public class OrderMongoRepository : BaseRepository<int, Order>
    {
        public OrderMongoRepository(MongoDbOperations<int, Order> operations):base(operations)
        {

        }
    }
}

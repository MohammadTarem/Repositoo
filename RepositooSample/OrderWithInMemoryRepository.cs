using System;
using Repositoo;
namespace RepositooSample
{
    public class OrderWithInMemoryRepository : BaseRepository<int, Order>
    {
        public OrderWithInMemoryRepository(InMemoryOperations<int, Order> operations) : base(operations)
        {

        }
    }
}

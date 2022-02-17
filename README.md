# Repositoo

This library implements the Repository pattern. It uses bridge pattern to seperate the details of data layer from domain objects.

The core interface is IDatabaseOperations<Id, TEntity> . It abstracts add, update and remove operations. The interface is drived from  IQueryable<TEntity> to enable efficient querying infrastructure.

Curently three implemetations has been added. One for SQL Server, one for MongoDb and the other for in memory repository.


## Aggrigate Root

The repository works best for aggrigate objects with tree-like structure. 

## How to Create a Repository

```
// Domain model (the aggrigate) is Order object with integer Id

// For SQL
public class OrdersSqlRepository : BaseRepository<int, Order>
{
    public OrdersSqlRepository( SqlOperations<int, Order> operations ) : base(operations)
    {
        
    }
}

// Init 
// DbContext context;
repository = new OrdersSqlRepository
(
    new SqlOperations<int, Order>( context, o => o.Id)
);

// For MongoDb
public class OrderMongoRepository : BaseRepository<int, Order>
{
    public OrderMongoRepository( MongoDbOperations<int, Order> operations ):base(operations)
    {

    }
}

// Init 
// IMongoClient client;
repository = new OrderMongoRepository
(
    new MongoDbOperations<int, Order>( client , "Orders", o => o.Id)

    
// For InMemory
services.AddSingleton<OrderWithInMemoryRepository>
(
    (s) => new OrderWithInMemoryRepository
        (
            new InMemoryOperations<int, Order>(o => o.Id)
        )
);


```




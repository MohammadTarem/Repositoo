using Repositoo;

// Spin SqlExpress in docker
// docker run --rm -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Sa@12345" -e "MSSQL_PID=Express" -p 1433:1433  mcr.microsoft.com/mssql/server:latest

namespace RepositooSample
{
    public class OrdersSqlRepository : BaseRepository<int, Order>
    {
        
        public OrdersSqlRepository( SqlOperations<int, Order> operations) : base(operations)
        {
           
        }
        
    }
}

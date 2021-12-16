using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repositoo;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace RepositooSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderWithSqlController : ControllerBase
    {


        private readonly ILogger<OrderWithSqlController> _logger;
        private OrdersContext Context;
        private OrdersSqlRepository repo;

        

        public OrderWithSqlController( OrdersContext context, ILogger<OrderWithSqlController> logger)
        {
            _logger = logger;
            Context = context;
            repo = new OrdersSqlRepository(
               new SqlOperations<int, Order>(Context, o => o.Id)
            );
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            

            var data = repo.Include(o => o.OrderDetails).FirstOrDefault(o => o.Id == id);
            return Ok(data);
        }

        [HttpPost("{number}")]
        public IActionResult Post(string number)
        {

            var id = repo.Add(new Order { Number = number, Description = "" });
            return Ok(new { id });

        }

        [HttpPut("orderdetails/{id}/{item}/{quantity}")]
        public IActionResult AddOrderDetails(int id, string item, int quantity)
        {

            var order = repo.FirstOrDefault( o => o.Id == id);
            
            order.OrderDetails.Add(new OrderDetail { Item = item, Quanitity = quantity });
            var Id = repo.Update(order);
            return Ok(new { Id });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var order = repo.FirstOrDefault(o => o.Id == id);
            var Id = repo.Delete(order);
            return Ok(new { Id });
        }

    }
}

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Repositoo;

namespace RepositooSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderWithMongoController : ControllerBase
    {
        
        private OrderMongoRepository repo;

        public OrderWithMongoController(IMongoClient client)
        {

            repo = new OrderMongoRepository
            (
                new MongoDbOperations<int, Order>(client, "Orders", o => o.Id)
            );
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            
            var data = repo.FirstOrDefault(o => o.Id == id);
            
            return Ok(data);
        }

        [HttpPost("{number}")]
        public IActionResult Post(string number)
        {

            var id = repo.Add(new Order { Id = 1, Number = number, Description = "" });
            return Ok(new { id });

        }

        [HttpPut("orderdetails/{id}/{item}/{quantity}")]
        public IActionResult AddOrderDetails(int id, string item, int quantity)
        {

            var order = repo.FirstOrDefault(o => o.Id == id);

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

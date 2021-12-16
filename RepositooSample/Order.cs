using System;
using System.Collections.Generic;

namespace RepositooSample
{
    public class Order 
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }
    }
}

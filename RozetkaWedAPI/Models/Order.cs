using RozetkaWedAPI.Models;
using System;
using System.Collections.Generic;

namespace RozetkaWebAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string BuyerName { get; set; }
        public string BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
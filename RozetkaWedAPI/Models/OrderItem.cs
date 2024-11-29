﻿using RozetkaWedAPI.Models;

namespace RozetkaWebAPI.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public Order Order { get; set; }
    }
}
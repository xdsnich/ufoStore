﻿using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Text.Json.Serialization;
namespace ufoShopBack.Data.Entities
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }
        public Product Product { get; set; }

    }
}

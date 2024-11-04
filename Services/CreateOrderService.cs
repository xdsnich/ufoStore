using ufoShopBack.Data.Entities;
using ufoShopBack.DTOs;
using ufoShopBack.Data;
namespace ufoShopBack.Services
{
    public class CreateOrderService
    {
        private readonly Context _context;
        public CreateOrderService(Context context)
        {
            _context = context;
        }

        public async Task<Order> OrderCreationAsync(int userId, List<OrderItemDTO> orderItems)
        {
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = orderItems.Sum(item => item.Price * item.Quantity),
                OrderItems = orderItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.SaveChanges();
            

            return order;
        }
    }
}

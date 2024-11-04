using ufoShopBack.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ufoShopBack.Data;
using ufoShopBack.DTOs;
namespace ufoShopBack.Services

{
    public class ShowOrderService
    {
        private readonly Context _context;
        private readonly DbSet<OrderItem> _orderItemSet;
        private readonly DbSet<Order> _orderSet;
        public ShowOrderService(Context context)
        {
            _context = context;
            _orderItemSet = _context.Set<OrderItem>();
            _orderSet = _context.Set<Order>();

        }
        public async Task<List<OrderDTO>> ShowOrderAsync(User user)
        {
            var userOrders = await _context.Orders
                .Where(x => x.UserId == user.Id)
                .Select(order => new OrderDTO
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Items = order.OrderItems.Select(item => new OrderItemDTO
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    }).ToList()

                }).ToListAsync();


            return userOrders;
        }
    }
}

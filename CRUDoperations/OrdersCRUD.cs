using ufoShopBack.Abstract;
using ufoShopBack.Data.Entities;
using ufoShopBack.Repos;
using ufoShopBack.Data;
using System.Runtime.InteropServices;
namespace ufoShopBack.CRUDoperations
{
    public class OrdersCRUD : ICRUDoperations<Order>
    {
        private readonly Context _context;
        private readonly GenericCRUDoperations<Order> _orderRepository;
        public OrdersCRUD(Context context) {
            _context = context;
            _orderRepository = new GenericCRUDoperations<Order>(context);
        }
        public async Task<List<Order>> GetAsync() => await _orderRepository.GetAsync();
        public async Task<Order> GetAsync(int id) => await _orderRepository.GetAsync(id);
        public async Task CreateAsync(Order order) => await _orderRepository.CreateAsync(order);
        public async Task UpdateAsync(Order order, int id) => await _orderRepository.UpdateAsync(order, id);
        public async Task DeleteAsync(int id) => await _orderRepository.DeleteAsync(id);

    }
}

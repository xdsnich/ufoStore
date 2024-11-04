using ufoShopBack.Data;
using ufoShopBack.Abstract;
using ufoShopBack.Data.Entities;
using ufoShopBack.Repos;
namespace ufoShopBack.CRUDoperations
{
    public class OrderItemCRUD : ICRUDoperations<OrderItem>
    {
        private readonly Context _context;
        private readonly GenericCRUDoperations<OrderItem> _orderItemRepository;
        public OrderItemCRUD(Context context)
        {
            _context = context;
            _orderItemRepository = new GenericCRUDoperations<OrderItem>(_context);
        }
        public async Task<List<OrderItem>> GetAsync() => await _orderItemRepository.GetAsync();
        public async Task<OrderItem> GetAsync(int id) => await _orderItemRepository.GetAsync(id);
       // public async Task<OrderItem> GetAsync(string name) => await _orderItemRepository.GetAsync(name);
        public async Task CreateAsync(OrderItem orederItem) => await _orderItemRepository.CreateAsync(orederItem);
        public async Task UpdateAsync(OrderItem orderItem, int id) => await _orderItemRepository.UpdateAsync(orderItem, id); 
        public async Task DeleteAsync(int id) => await _orderItemRepository.DeleteAsync(id);
    }
}

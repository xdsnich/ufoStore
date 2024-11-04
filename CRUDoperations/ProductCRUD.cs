using Microsoft.EntityFrameworkCore;
using ufoShopBack.Data;
using ufoShopBack.Abstract;
using ufoShopBack.Repos;
using ufoShopBack.Data.Entities;
namespace ufoShopBack.CRUDoperations
{
    public class ProductCRUD : ICRUDoperations<Product>
    {
        private readonly Context _contextProducts;
        private readonly GenericCRUDoperations<Product> _productRepository;

        public ProductCRUD(Context contextProducts)
        {
            _contextProducts = contextProducts;
            _productRepository = new GenericCRUDoperations<Product>(_contextProducts);
        }

        public async Task<List<Product>> GetAsync() => await _productRepository.GetAsync();
        public async Task<Product> GetAsync(int id) => await _productRepository.GetAsync(id);
       // public async Task<Product> GetAsync(string name) => await _productRepository.GetAsync(name);

        public async Task CreateAsync(Product product) => await _productRepository.CreateAsync(product);
        public async Task UpdateAsync(Product product, int id) => await _productRepository.UpdateAsync(product, id);
        public async Task DeleteAsync(int id) => await _productRepository.DeleteAsync(id);
    }
}

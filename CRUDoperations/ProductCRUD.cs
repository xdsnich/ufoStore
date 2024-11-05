using Microsoft.EntityFrameworkCore;
using ufoShopBack.Data;
using ufoShopBack.Abstract;
using ufoShopBack.Repos;
using ufoShopBack.Data.Entities;
using ufoShopBack.Services;
namespace ufoShopBack.CRUDoperations
{
    public class ProductCRUD : ICRUDoperations<Product>
    {
        private readonly Context _contextProducts;
        private readonly DbSet<Product> _products;
        private readonly GenericCRUDoperations<Product> _productRepository;
        private readonly ProductServices _productServices;

        public ProductCRUD(Context contextProducts)
        {
            _contextProducts = contextProducts;
            _products = _contextProducts.Set<Product>();
            _productRepository = new GenericCRUDoperations<Product>(_contextProducts);

        }

        public async Task<List<Product>> GetAsync() => await _productRepository.GetAsync();
        public async Task<Product> GetAsync(int id) => await _productRepository.GetAsync(id);
        public async Task<Product> GetAsyncByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }
            var productFromDb = await _products.FirstOrDefaultAsync(p => p.Name == name);
            if (productFromDb == null) {
                return null;
            }
            return productFromDb;
        }

        public async Task CreateAsync(Product product) {
            await _productServices.SetNewPriceAsync(product.Price, product.Id);
            await _productRepository.CreateAsync(product);
        } 
        public async Task UpdateAsync(Product product, int id) => await _productRepository.UpdateAsync(product, id);
        public async Task DeleteAsync(int id) => await _productRepository.DeleteAsync(id);
    }
}

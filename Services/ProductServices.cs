using ufoShopBack.Data.Entities;
using ufoShopBack.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
namespace ufoShopBack.Services
{
    public class ProductServices
    {
        private readonly Context _context;
        public ProductServices(Context context) 
        { 
            _context = context;
        }

        public async Task IncrementViewCountAsync(int productId)
        {
            var product = await _context.FindAsync<Product>(productId);
            if (product != null)
            {
                product.ViewNumber = (product.ViewNumber ?? 0) + 1;
                await _context.SaveChangesAsync();
            }
        }
       public async Task FormRatingAsync(int productId, int productRate)
        {   
            var product = await _context.FindAsync<Product>(productId);
            if (product != null)
            { 
                if(product.Rating == null)
                {
                    product.Rating = productRate;
                    await _context.SaveChangesAsync();
                    return;
                }
                _context.Rating.Add(new RatingList { ProductId = productId, RatingNum = productRate });
                _context.SaveChanges();

                var ratings = _context.Rating.Where(r => r.ProductId == productId)
                    .Select(r => r.RatingNum);
                int sumRating = await ratings.SumAsync();
                int count = await ratings.CountAsync();
                double average = (double)sumRating / count;
                product.Rating = average;
                await _context.SaveChangesAsync();
            }
        }
        public async Task SetNewPriceAsync(int newPrice, int productId)
        {
            var product = await _context.FindAsync<Product>(productId);
            if(product == null)
            {
                return;
            }
            product.OldPrice = product.Price;
            product.Price = newPrice;
            await _context.SaveChangesAsync();
        }
    }
}

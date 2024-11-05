using Microsoft.EntityFrameworkCore;
using ufoShopBack.Abstract;
using ufoShopBack.Data;
using ufoShopBack.Services;
namespace ufoShopBack.Repos
{

    public class GenericCRUDoperations<T> : ICRUDoperations<T> where T : class
    {
        private readonly Context _context;
        private readonly DbSet<T> _dbSet;
        public GenericCRUDoperations( Context context)
        {
            _context = context;
            _dbSet = _context.Set<T>();

        }
        public async Task<List<T>> GetAsync() => await _dbSet.ToListAsync();

        public async Task<T> GetAsync(int id)
        {
            var entityFromDb = await _dbSet.FindAsync(new object[] { id });
            if (entityFromDb == null)
            {
                return entityFromDb!;
            }
            return entityFromDb;
        }
        

        public  async Task CreateAsync(T entity)
        {

            await _dbSet.AddAsync(entity);
            await SaveAsync();

        }

        public async Task UpdateAsync(T entity, int id)
        {
            var entityFromDb = await _dbSet.FindAsync(entity);
            if (entityFromDb == null)
            {
                return;
            }
            _context.Entry(entityFromDb).CurrentValues.SetValues(entity);
            await SaveAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var entityFromDb = await _dbSet.FindAsync(new object[] { id });
            if (entityFromDb == null)
            {
                return;
            }
            _dbSet.Remove(entityFromDb);
            await SaveAsync();


        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
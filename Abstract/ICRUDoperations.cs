namespace ufoShopBack.Abstract
{
    public interface ICRUDoperations <T>{
        Task CreateAsync(T item);
        Task<List<T>> GetAsync();
        Task<T> GetAsync(int id);
       // Task<T> GetAsync(string name);
        Task UpdateAsync(T item, int id);
        Task DeleteAsync(int id);
        
    }
}

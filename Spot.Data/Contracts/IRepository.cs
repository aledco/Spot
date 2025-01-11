namespace Spot.Data.Contracts
{
    public interface IRepository<T>
    {
        Task<T?> GetAsync<K>(K id);

        Task<IList<T>> GetAllAsync();

        Task<T> SaveAsync(T entity);
    }
}

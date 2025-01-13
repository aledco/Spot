namespace Spot.Data.Contracts
{
    public interface IRepository<T>
    {
        Task<T?> GetAsync(int id);

        Task<IList<T>> GetAllAsync();

        Task<T> SaveAsync(T entity);

        Task SaveRangeAsync(IEnumerable<T> entities);

        Task DeleteAsync(T entity);

        Task DeleteRangeAsync(IEnumerable<T> entities);
    }
}

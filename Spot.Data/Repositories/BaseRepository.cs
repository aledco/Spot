using Microsoft.EntityFrameworkCore;
using Spot.Data.Contracts;

namespace Spot.Data.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationContext _context;

        public BaseRepository(ApplicationContext context)
        {
            this._context = context;
        }

        public async Task<T?> GetAsync<K>(K id)
        {
            return await this._context.FindAsync<T>(id);
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await this._context.Set<T>().ToListAsync();
        }

        public async Task<T> SaveAsync(T entity)
        {
            var entry = await this._context.AddAsync(entity);
            await this._context.SaveChangesAsync();
            return entry.Entity;
        }
    }
}

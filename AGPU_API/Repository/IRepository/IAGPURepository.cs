using System.Linq.Expressions;

namespace AGPU_API.Repository.IRepository
{
    public interface IAGPURepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T,bool>> filter = null, bool tracked = true);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        Task AddAsync(T entity);
        Task Remove(T entity); //no async method for Remove
        Task SaveAsync();
    }
}

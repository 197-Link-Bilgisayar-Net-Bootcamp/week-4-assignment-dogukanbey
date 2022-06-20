
using System.Linq.Expressions;

namespace week4.Data.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetAll();


        Task AddAsync(T entity);

        void Update(T entity);
        void Delete(T entity);

        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
    }
}

namespace week4.Service.Services
{
    public interface IService<T> where T : class
    {

        Task<T> AddAsync(T entity);


        Task<IEnumerable<T>> GetAllAsync();

       Task<T> GetByIdAsync(int id);

       Task RemoveAsync(T entity);

       Task UpdateAsync(T entity);





    }
}

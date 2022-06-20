using week4.Data.Models;
using week4.Data.Repositories;

namespace week4.Data.UnitOfWork
{
    public interface IUnitOfWork    
    {

        IProductRepository Products { get; }
       
        Task CommitAsync();
        void Commit();
     
    }
}

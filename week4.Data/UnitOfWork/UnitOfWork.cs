using week4.Data.Models;
using week4.Data.Repositories;
 

namespace week4.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AppDbContext _context;

        private ProductRepository _productRepository;
      


        public IProductRepository Products => _productRepository = _productRepository ?? new ProductRepository(_context);
    
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        
        }


 

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }






    }
}

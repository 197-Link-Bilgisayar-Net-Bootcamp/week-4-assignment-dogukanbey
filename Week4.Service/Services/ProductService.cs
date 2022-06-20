using AutoMapper;
using week4.Data.Models;
using week4.Data.Repositories;
using week4.Data.UnitOfWork;
using week4.Service.Services;

namespace week4.Service.Services
{
    public class ProductService : Service<Product>, IProductService
    {
       
        public ProductService(IGenericRepository<Product> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {

            
        }


    }
}
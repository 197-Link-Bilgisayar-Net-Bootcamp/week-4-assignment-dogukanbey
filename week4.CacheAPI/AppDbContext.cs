using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using week4.Data.Configurations;
using week4.Data.Models;

namespace week4.CacheAPI
{

    public class CacheDbContext : DbContext
    {
        public CacheDbContext(DbContextOptions<CacheDbContext> options) : base(options)
        {

        }

 
        public DbSet<Product> Products { get; set; }
   

 

    
    }
}

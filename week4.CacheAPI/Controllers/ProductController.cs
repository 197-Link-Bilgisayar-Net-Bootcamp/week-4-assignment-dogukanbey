using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using week4.CacheAPI.Models;
using week4.CacheAPI.Services;
using week4.Data;
using week4.Data.Models;

namespace week4.CacheAPI.Controllers
{   

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        
        private readonly AppDbContext _dbcontext;
        private readonly IMemoryCache _cache;
        private readonly IRedisCacheService _redisCacheService;


        public List<Product> Products = new List<Product>();


        public ProductController(AppDbContext dbcontext, IMemoryCache cache, IRedisCacheService redisCacheService)
        {

            _dbcontext = dbcontext;
            _cache = cache;
            _redisCacheService = redisCacheService;
        }




        public List<Product> GetAllProducts()
        {
           
            
            Products = _dbcontext.Products.OrderBy(i => i.Id).ToList();
            _cache.Set("products", Products, 
            new MemoryCacheEntryOptions {Priority = CacheItemPriority.Low, SlidingExpiration = TimeSpan.FromSeconds(60) });

            return Products;

        }


        [HttpPost("cache/{key}")]
        public async Task<IActionResult> Get(string key)
        {
            return Ok(await _redisCacheService.GetValueAsync(key));
        }




        [HttpPost("cache")]
        public async Task<IActionResult> Post([FromBody] CacheRequestModel model)
        {
            await _redisCacheService.SetValueAsync(model.Key, model.Value);
            return Ok();
        }




        [HttpDelete("cache/{key}")]
        public async Task<IActionResult> Delete(string key)
        {
            await _redisCacheService.Clear(key);
            return Ok();
        }
















    }
 

}

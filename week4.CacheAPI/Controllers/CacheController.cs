using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using System.Text.Json;
using week4.CacheAPI.Models;
using week4.CacheAPI.Services;



namespace week4.CacheAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IRedisCacheService _redisCacheService;


  

        public CacheController(IRedisCacheService redisCacheService)
        {


            _redisCacheService = redisCacheService;
        }

 
        

        [HttpPost("InsertAKeyString")]
        public async Task<IActionResult> Post([FromBody] CacheRequestModel model)
        {
            await _redisCacheService.SetStringAsync(model.Key, model.Value);
            return Ok();
        }


        [HttpPost("GetByKeyString")]
        public async Task<IActionResult> Get(string key)
        {
            return Ok(await _redisCacheService.GetStringAsync(key));
        }

        [HttpPost("InsertAKeyList")]
        public async Task<IActionResult> PostList([FromBody] CacheRequestModel model)
        {
            await _redisCacheService.ListLeftPushAsync(model.Key, model.Value);
            return Ok();
        }


        [HttpPost("GetByKeyListRight")]
        public async Task<IActionResult> GetListRight(string key)
        {
            return Ok(await _redisCacheService.ListRightPopAsync(key));
        }

        [HttpPost("GetByKeyListLeft")]
        public async Task<IActionResult> GetListLeft(string key)
        {
            return Ok(await _redisCacheService.ListLeftPopAsync(key));
        }


        [HttpPost("InsertAKeySet")]
        public async Task<IActionResult> PostSet([FromBody] CacheRequestModel model)
        {
            await _redisCacheService.SetSetAsync(model.Key, model.Value);
            return Ok();
        }


        [HttpPost("GetByKeySet")]
        public IActionResult GetSet(string key)
        {
            return Ok( _redisCacheService.GetSetAsync(key));
        }


        [HttpPost("InsertAKeySorted")]
        public async Task<IActionResult> PostSetSorted([FromBody] CacheRequestModel model)
        {
            await _redisCacheService.SetSortedSetAsync(model.Key, model.Value);
            return Ok();
        }


        [HttpPost("GetByKeySorted")]
        public IActionResult GetSetSorted(string key)
        {
            return Ok(_redisCacheService.GetSortedAsync(key));
        }


  

        [HttpDelete("DeleteAll")]
        public void DeleteAll()
        {
            _redisCacheService.ClearAll();

        }

        [HttpDelete("DeleteByKey")]
        public async Task<IActionResult> Delete(string key)
        {
            await _redisCacheService.Clear(key);
            return Ok();
        }







    }
}
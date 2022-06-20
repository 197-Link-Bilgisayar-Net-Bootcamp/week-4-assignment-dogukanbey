using StackExchange.Redis;
using System.Text.Json;

namespace week4.CacheAPI.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redisCnn;
        private readonly IDatabase _cache;
 

        public RedisCacheService(IConnectionMultiplexer redisCnn)
        {
            _redisCnn = redisCnn;
            _cache = redisCnn.GetDatabase();
        }

        

        #region Redis String

        public async Task<T> GetStringAsync<T>(string key, Func<Task<T>> action) where T : class
        {
            var result = await _cache.StringGetAsync(key);
            return JsonSerializer.Deserialize<T>(result);
        }

        public async Task<bool> SetStringAsync(string key, string value)
        {
            return await _cache.StringSetAsync(key, value, TimeSpan.FromHours(12));
        }

        #endregion

        #region Redis List


        public async Task<T> GetListAsync<T>(string key, int index) where T : class
        {
            var result = await _cache.ListGetByIndexAsync(key, index );
            return JsonSerializer.Deserialize<T>(result);
        }

        public  Task SetListAsync(string key, int index, string value)
        {
            return  _cache.ListSetByIndexAsync(key, index, value);
        }






        #endregion

        #region Redis Set


        public async Task<bool> SetSetAsync(string key, string value)
        {
            return await _cache.SetAddAsync(key, value);
        }



        #endregion

        #region Redis Sorted Set


        Random rnd = new Random(1000);
        public async Task<bool> SetSortedSetAsync(string key, string value)
        {
            return await _cache.SortedSetAddAsync(key, value, rnd.Next());
        }



        #endregion

        #region Redis Hash

        public   Task<HashEntry[]> GetHashAsync<T>(string key, Func<Task<T>> action, HashEntry[] result) where T : class
        {

            return _cache.HashGetAllAsync(key);
        }

        public async Task<bool> SetHashAsync(string key, string value)
        {
            return await _cache.StringSetAsync(key, value, TimeSpan.FromHours(12));
        }


        #endregion


        public void ClearAll()
        {
            var endpoints = _redisCnn.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = _redisCnn.GetServer(endpoint);
                server.FlushAllDatabases();
            }
        }

    }
}
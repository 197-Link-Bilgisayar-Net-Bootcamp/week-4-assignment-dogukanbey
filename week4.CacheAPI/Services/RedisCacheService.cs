using StackExchange.Redis;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
 
using Microsoft.AspNetCore.Mvc;
 

namespace week4.CacheAPI.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redisCnn;
        private readonly IDatabase _cache;
        private TimeSpan ExpireTime => TimeSpan.FromDays(1);


        public RedisCacheService(IConnectionMultiplexer redisCnn)
        {
            _redisCnn = redisCnn;
            _cache = redisCnn.GetDatabase();
        }



        #region Redis String

        public async Task<string> GetStringAsync(string key)
        {
            return await _cache.StringGetAsync(key);
        }

        public async Task<bool> SetStringAsync(string key, string value)
        {
            return await _cache.StringSetAsync(key, value, TimeSpan.FromHours(12));
        }



   



        #endregion

        #region Redis List






        public async Task<string> ListRightPopAsync(string redisKey)
        {
            return await _cache.ListRightPopAsync(redisKey);
        }

        public async Task<string> ListLeftPopAsync(string redisKey)
        {
            return await _cache.ListLeftPopAsync(redisKey);
        }

        public async Task<long> ListLeftPushAsync(string key, string value )
        {
            return await _cache.ListLeftPushAsync(key, value);
        }
        #endregion

        #region Redis Set


        public async Task<bool> SetSetAsync(string key, string value)
        {
            return await _cache.SetAddAsync(key, value);
        }


        public  string[] GetSetAsync(string key)
        {
            return _cache.SetMembers(key).ToStringArray();              
        
        }
        #endregion

        #region Redis Sorted Set
        
        
        Random score = new Random(1000);
        public async Task<bool> SetSortedSetAsync(string key, string value)
        {
            return await _cache.SortedSetAddAsync(key, value, score.Next());
        }


        public string[] GetSortedAsync(string key)
        {
            return _cache.SortedSetRangeByRank(key).ToStringArray();

        }

        #endregion




        #region Redis Hash


        public Dictionary<string,string> GetHashAsync(string hashMapName)
        {
            Dictionary<string,string> result = new Dictionary<string, string>();
            var allHash =   _cache.HashGetAll(hashMapName);
            foreach (var item in allHash)
            {
                result.Add(item.Name, item.Value);
            }

            return result;

        }


        public Task SetHashAsync(string hashMapName, string key, string value)
        {
            return  _cache.HashSetAsync(hashMapName,   key,   value);
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

        public async Task<bool> Clear(string key)
        {
           return await _cache.KeyDeleteAsync(key);
        }

      
    }
}
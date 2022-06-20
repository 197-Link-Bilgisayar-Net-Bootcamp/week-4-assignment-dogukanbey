using StackExchange.Redis;

namespace week4.CacheAPI.Services
{
    public interface IRedisCacheService
    {



        Task<T> GetStringAsync<T>(string key, Func<Task<T>> action);
        Task<bool> SetStringAsync(string key, string value);
        Task<T> GetListAsync<T>(string key, int index);
        Task SetListAsync(string key, int index, string value);

        Task<bool> SetSetAsync(string key, string value);
        Task<bool> SetSortedSetAsync(string key, string value);
        Task<HashEntry[]> GetHashAsync<T>(string key, Func<Task<T>> action, HashEntry[] result)
        Task<bool> SetHashAsync(string key, string value);

        void ClearAll();
      
    }
}

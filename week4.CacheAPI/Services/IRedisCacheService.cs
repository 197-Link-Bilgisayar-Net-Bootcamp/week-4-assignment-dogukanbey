using StackExchange.Redis;

namespace week4.CacheAPI.Services
{
    public interface IRedisCacheService
    {

 

        Task<string> GetStringAsync(string key);
        Task<bool> SetStringAsync(string key, string value);

        Task<string> ListRightPopAsync(string key);
        Task<string> ListLeftPopAsync(string key);


        Task<long> ListLeftPushAsync(string key,  string value);

 
        Task<bool> SetSetAsync(string key, string value);


        string[] GetSetAsync(string key);


        Task<bool> SetSortedSetAsync(string key, string value);

        string[] GetSortedAsync(string key);


        Task SetHashAsync(string hashMapName, string key, string value);

        Dictionary<string,string> GetHashAsync(string hashMapName);


        void ClearAll();

        Task<bool> Clear(string key);
    }
}

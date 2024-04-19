using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using Commons;

namespace ASPNETCore
{
    public class MemoryCacheHelper : IMemoryCacheHelper
    {
        public readonly MemoryCache _memoryCache;
        public MemoryCacheHelper(MemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 数据验证
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        public static void ValidateValueType<TResult>()
        {
            // 排除IEnumerable、IQueryable的类型因为IEnumerable、IQueryable等有延迟执行的问题，造成麻烦，因此禁止用这些类型
            Type typeResult = typeof(TResult);
            if (typeResult.IsGenericType)
            {
                typeResult = typeResult.GetGenericTypeDefinition();
            }

            if (typeResult == typeof(IEnumerable<>) || typeResult == typeof(IEnumerable)
                || typeResult == typeof(IQueryable<>) || typeResult == typeof(IQueryable)
                || typeResult == typeof(IAsyncEnumerable<TResult>))
            {
                throw new InvalidOperationException($"TResult of {typeResult} is not allowed, please use List<T> or T[] instead.");
            }
        }

        private static void InitCacheEntry(ICacheEntry entry, int baseExpireSeconds)
        {
            double sec = Random.Shared.NextDouble(baseExpireSeconds, baseExpireSeconds * 2);
            TimeSpan expiration = TimeSpan.FromSeconds(sec);
            entry.AbsoluteExpirationRelativeToNow = expiration;
        }

        public TResult? GetOrCreate<TResult>(string cacheKey, Func<ICacheEntry, TResult?> valueFactory, int expireSeconds = 60)
        {
            ValidateValueType<TResult>();
            // 因为IMemoryCache保存的是一个CacheEntry，所以null值也认为是合法的，因此返回null不会有“缓存穿透”的问题,8.0 Value 默认可空了
            if (!_memoryCache.TryGetValue(cacheKey, out TResult? result))
            { 
                using ICacheEntry entry = _memoryCache.CreateEntry(cacheKey);
                InitCacheEntry(entry, expireSeconds);
                result = valueFactory(entry);
                entry.Value = result;
            }

            return result;
        }

        public async Task<TResult?> GetOrCreateAsync<TResult>(string cacheKey, Func<ICacheEntry, Task<TResult?>> valueFactory, int expireSeconds = 60)
        {
            ValidateValueType<TResult>();
            if (!_memoryCache.TryGetValue(cacheKey, out TResult result))
            {
                using ICacheEntry cacheEntry = _memoryCache.CreateEntry(cacheKey);
                InitCacheEntry(cacheEntry, expireSeconds);
                result = await valueFactory(cacheEntry);
                cacheEntry.Value = result;
            }
            return result;
        }

        public void Remove(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }
    }
}

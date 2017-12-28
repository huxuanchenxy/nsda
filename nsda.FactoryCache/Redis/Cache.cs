using System;

namespace nsda.Cache.Redis
{
    public class Cache : ICache
    {
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        public T GetCache<T>(string cacheKey) where T : class
        {
            return RedisCache.Get<T>(cacheKey);
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        public void RemoveCache(string cacheKey)
        {
            RedisCache.Remove(cacheKey);
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public void RemoveCache()
        {
            RedisCache.RemoveAll();
        }

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        public void WriteCache<T>(string cacheKey,T value) where T : class
        {
            //配置成与webcache相同时间
            WriteCache(cacheKey, value,  DateTime.Now.AddMinutes(10));
        }

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="value">对象数据</param>
        /// <param name="cacheKey">键</param>
        /// <param name="expireTime">到期时间</param>
        public void WriteCache<T>(string cacheKey, T value,  DateTime expireTime) where T : class
        {
            RedisCache.Set(cacheKey, value, expireTime);
        }
    }
}
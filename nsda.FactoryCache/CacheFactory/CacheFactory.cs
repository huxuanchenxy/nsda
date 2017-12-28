namespace nsda.Cache.CacheFactory
{
    public class CacheFactory
    {
        /// <summary>
        /// 定义通用的Repository
        /// </summary>
        /// <returns></returns>
        public static ICache Cache(string cacheType)
        {
            switch (cacheType)
            {
                case "Redis":
                    return new Redis.Cache();

                case "Cache":
                    return new Cache();

                default:
                    return new Cache();
            }
        }
    }
}
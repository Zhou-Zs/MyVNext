using System.Security.Claims;

namespace JWT
{
    public class JWTOptions
    {
        /// <summary>
        /// 发行者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 订阅者
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 秘钥
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public int ExpireSeconds { get; set; }
    }
}

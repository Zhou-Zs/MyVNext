using System.Data;

namespace AnyDBConfigProvider
{
    public class DBConfigOptions
    {
        public Func<IDbConnection> CreateDbConnection { get; set; }

        /// <summary>
        /// 配置表表名
        /// </summary>
        public string TableName { get; set; } = "T_Config";

        /// <summary>
        /// 是否重新加载
        /// </summary>
        public bool ReloadOnChange { get; set; } = false;

        /// <summary>
        /// 重新加在间隔时间
        /// </summary>
        public TimeSpan? ReloadInterval { get; set; }

    }
}

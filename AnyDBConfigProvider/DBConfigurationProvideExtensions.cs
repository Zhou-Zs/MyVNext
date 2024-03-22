using Microsoft.Extensions.Configuration;
using System.Data;

namespace AnyDBConfigProvider
{
    public static class DBConfigurationProvideExtensions
    {
        public static IConfigurationBuilder AddDbConfiguration(this IConfigurationBuilder builder, DBConfigOptions setup)
        {
            return builder.Add(new DBConfigurationSource(setup));
        }

        public static IConfigurationBuilder AddDbConfiguration(this IConfigurationBuilder builder, Func<IDbConnection> createDbConnection, string tableName = "T_Configs", bool reloadOnChange = false, TimeSpan? reloadInterval = null)
        {
            return AddDbConfiguration(builder, new DBConfigOptions
            {
                CreateDbConnection = createDbConnection,
                TableName = tableName,
                ReloadOnChange = reloadOnChange,
                ReloadInterval = reloadInterval
            });
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using AnyDBConfigProvider;
using System.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace CommnoInitializer
{
    public class WebApplicationBuilderExtensions
    {
        public static void ConfigureDbConfiguration(this WebApplicationBuilder builder)
        {
            //不能使用ConfigureAppConfiguration中的configBuilder去读取配置，否则就循环调用了，因此这里直接自己去读取配置文件
            //var configRoot = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            //string connStr = configRoot.GetValue<string>("DefaultDB:ConnStr");

            builder.Host.ConfigureAppConfiguration((hostCtx, configBuild) =>
            {
                string connStr = builder.Configuration.GetValue<string>("DefaultDB:ConnStr");
                configBuild.AddDbConfiguration(() => new SqlConnection(connStr), reloadOnChange: true, reloadInterval: TimeSpan.FromSeconds(5));
            });
        }

        public static void ConfigExtraServices(this WebApplicationBuilder builder, InitializerOptions initializerOptions)
        {
            IServiceCollection services = builder.Services;
            IConfiguration configuration = builder.Configuration;

        }

    }
}

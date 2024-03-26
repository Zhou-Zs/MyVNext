using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Infrastructure
{
    public static class EFCoreInitializerHelper
    {
        /// <summary>
        /// 自动为所有的DbContext注册连接配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="builder"></param>
        /// <param name="assemblies"></param>
        public static IServiceCollection AddAllDbContexts(this IServiceCollection services, Action<DbContextOptionsBuilder> builder,IEnumerable<Assembly> assemblies)
        {
            Type[] types = new Type[] { typeof(IServiceCollection), typeof(Action<DbContextOptionsBuilder>), typeof(ServiceLifetime), typeof(ServiceLifetime) };

            var methodAddDbContext = typeof(EntityFrameworkServiceCollectionExtensions).GetMethod(nameof(EntityFrameworkServiceCollectionExtensions.AddDbContext), 1, types);

            foreach (var asmToLoad in assemblies)
            {
                Type[] typesInAsm = asmToLoad.GetTypes();
                var dbCtxTypes= typesInAsm.Where(t => !t.IsAbstract && typeof(DbContext).IsAssignableFrom(t));
                foreach (var dbCtxType in dbCtxTypes)
                {
                    var methodGenericAddDbContext = methodAddDbContext.MakeGenericMethod(dbCtxType);

                    methodGenericAddDbContext.Invoke(null, new object[] { services, builder, ServiceLifetime.Scoped, ServiceLifetime.Scoped });
                }
            }
            return services;
        }
    }
}

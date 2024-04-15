using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace MediatR
{
    public static  class MediatorExtensions
    {
        /// <summary>
        /// 把rootAssembly及直接或者间接引用的程序集（排除系统程序集）中的MediatR 相关类进行注册
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddMediatR(this IServiceCollection services,IEnumerable<Assembly> assemblies)
        {
            return services.AddMediatR(assemblies.ToArray());
        }

        //public static async Task DispatchDomainEventsAsync(this IMediator mediator,DbContext ctx)
        //{
        //  var domainEntities =  ctx.ChangeTracker.Entries<IDomianEvents>
        //}
    }
}

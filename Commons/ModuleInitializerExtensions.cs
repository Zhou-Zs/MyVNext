using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Commons
{
    public static class ModuleInitializerExtensions
    {
        /// <summary>
		/// 每个项目中都可以自己写一些实现了IModuleInitializer接口的类，在其中注册自己需要的服务，这样避免所有内容到入口项目中注册
		/// </summary>
		/// <param name="services"></param>
		/// <param name="assemblies"></param>
        public static IServiceCollection RunModuleInitializers(this IServiceCollection services,IEnumerable<Assembly> assemblies)
        {
            foreach (var asm in assemblies)
            {
                Type[] types = asm.GetTypes();
                var moduleInitializerTypes = types.Where(t => !t.IsAbstract && typeof(IModuleInitialiazer).IsAssignableFrom(t)); // 检测IModuleInitialiazer 是否能传递给当前的类型 能传递便是继承关系
                foreach (var implType in moduleInitializerTypes)
                {
                    var initialiazer = (IModuleInitialiazer?)Activator.CreateInstance(implType);
                    if (initialiazer == null)
                    {
                        throw new ApplicationException($"Cannot create {implType}");
                    }
                    initialiazer.Initialize(services);
                }
            }

            return services;
        }
    }
}

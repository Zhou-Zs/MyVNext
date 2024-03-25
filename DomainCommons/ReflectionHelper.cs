using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DomainCommons
{
    public class ReflectionHelper
    {
        public static IEnumerable<Assembly> GetAllReferencedAssblies(bool skipSystemAssemblies = true)
        { 
            Assembly? rootAssmbly = Assembly.GetEntryAssembly(); // 获取入口程序集
            if (rootAssmbly == null)
            {
                rootAssmbly = Assembly.GetCallingAssembly(); // 返回当前调用的程序集
            }

            var returnAssemblies = new HashSet<Assembly>(new AssemblyEquality());
            var loadedAsseblies = new HashSet<string>();
            var assembliseToCkeck = new Queue<Assembly>();
             assembliseToCkeck.Enqueue(rootAssmbly);

        }

       
    }

    class AssemblyEquality : IEqualityComparer<Assembly>
    {
        public bool Equals(Assembly? x, Assembly? y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            return AssemblyName.ReferenceMatchesDefinition(x.GetName(), y.GetName()); // 比较程序集名称
        }

        public int GetHashCode([DisallowNull] Assembly obj)
        {
            return obj.GetName().FullName.GetHashCode();
        }
    }
}

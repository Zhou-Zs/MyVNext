using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Reflection.Metadata;
using System.Diagnostics;


namespace Commons
{
    public static class ReflectionHelper
    {
        public static IEnumerable<Assembly> GetAllReferencedAssblies(bool skipSystemAssemblies = true)
        { 
            Assembly? rootAssmbly = Assembly.GetEntryAssembly(); // 获取入口程序集
            if (rootAssmbly == null)
            {
            }

                rootAssmbly = Assembly.GetCallingAssembly(); // 返回当前调用的程序集
            var returnAssemblies = new HashSet<Assembly>(new AssemblyEquality());
            var loadedAsseblies = new HashSet<string>();
            var assembliesToCheck = new Queue<Assembly>();
            assembliesToCheck.Enqueue(rootAssmbly);
            if (!(skipSystemAssemblies && IsSystemAssembly(rootAssmbly)))
            {
                if (IsValid(rootAssmbly))
                {
                    returnAssemblies.Add(rootAssmbly);
                }
            }

            while (assembliesToCheck.Any())
            { 
              var assemblyToCheck = assembliesToCheck.Dequeue();
                foreach (var reference in assemblyToCheck.GetReferencedAssemblies())
                {
                    if (!loadedAsseblies.Contains(reference.FullName))
                    {
                        var assembly = Assembly.Load(reference);
                        if (skipSystemAssemblies && IsSystemAssembly(rootAssmbly))
                        {
                            continue;
                        }
                        assembliesToCheck.Enqueue(assembly);
                        loadedAsseblies.Add(reference.FullName);
                        if (IsValid(assembly))
                        {
                            returnAssemblies.Add(assembly);
                        }
                    }
                }
            }

           var asmsInBaseDir = Directory.EnumerateFiles(AppContext.BaseDirectory,"*.dll",new EnumerationOptions {RecurseSubdirectories = true }); // 获取bin目录里面的dll，包含子目录
            foreach (var asmPath in asmsInBaseDir)
            {
                if (!IsManagedAssembly(asmPath))
                {
                    continue;
                }

                AssemblyName asmName = AssemblyName.GetAssemblyName(asmPath);

                if (returnAssemblies.Any(x => AssemblyName.ReferenceMatchesDefinition(x.GetName(), asmName))) // 检查当前程序集是否存在
                {
                    continue;
                }
                if (skipSystemAssemblies && IsSystemAssembly(asmPath))
                {
                    continue;
                }
                Assembly? asm = TryLoadAssembly(asmPath);
                if (asm == null)
                {
                    continue;
                }
                if (skipSystemAssemblies && IsSystemAssembly(asm))
                {
                    continue;
                }
                returnAssemblies.Add(asm);
            }
            return returnAssemblies.ToArray();
        }

        private static Assembly? TryLoadAssembly(string asmPath)
        {
            AssemblyName asmName = AssemblyName.GetAssemblyName(asmPath);
            Assembly? asm = null;
            try
            {
                asm = Assembly.Load(asmName);
            }
            catch (BadImageFormatException ex)
            {
                Debug.WriteLine(ex);
            }
            catch (FileLoadException ex)
            {
                Debug.WriteLine(ex);
            }

            if (asm == null)
            {
                try
                {
                    asm = Assembly.LoadFile(asmPath);
                }
                catch (BadImageFormatException ex)
                {
                    Debug.WriteLine(ex);
                }
                catch (FileLoadException ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            return asm;
        }

        static bool IsSystemAssembly(string asmPath)
        {
            var moduleDef = AsmResolver.DotNet.ModuleDefinition.FromFile(asmPath);
            var assembly = moduleDef.Assembly;
            if (assembly == null)
            {
                return false;
            }
            var asmCompanyAttr = assembly.CustomAttributes.FirstOrDefault(c => c.Constructor?.DeclaringType?.FullName == typeof(AssemblyCompanyAttribute).FullName);
            if (asmCompanyAttr == null)
            {
                return false;
            }
            var companyName = ((AsmResolver.Utf8String?)asmCompanyAttr.Signature?.FixedArguments[0]?.Element)?.Value;
            if (companyName == null)
            {
                return false;
            }
            return companyName.Contains("Microsoft");
        }

        /// <summary>
        /// 判断file这个是不是程序集
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        static bool IsManagedAssembly(string file)
        {
            using var fs = File.OpenRead(file);
            using PEReader peReader = new PEReader(fs); //  Creates a Portable Executable reader over a PE image stored in a stream.
            return peReader.HasMetadata && peReader.GetMetadataReader().IsAssembly;
        }

        /// <summary>
        /// 是否是微软等的官方Assembly
        /// </summary>
        /// <param name="asm"></param>
        /// <returns></returns>
        static bool IsSystemAssembly(Assembly asm)
        {
            var asmCompanyAttr = asm.GetCustomAttribute<AssemblyCompanyAttribute>();
            if (asmCompanyAttr == null)
            {
                return false;
            }
            else
            {
                string companyName = asmCompanyAttr.Company;
                return companyName.Contains("Microsoft");
            }
        }

        static bool IsValid(Assembly asm)
        {
            try {
                asm.GetTypes();
                asm.DefinedTypes.ToList();
                return true;
            }
            catch(ReflectionTypeLoadException)
            {
                return false;
            }
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

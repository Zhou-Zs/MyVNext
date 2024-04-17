using Microsoft.EntityFrameworkCore;

namespace ASPNETCore
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class UnitOfWorkAttribute : Attribute
    {
        public Type[] DbContextType { get; init; }
        public UnitOfWorkAttribute(params Type[] dbContextTypes)
        {
            DbContextType = dbContextTypes;
            foreach (var type in dbContextTypes)
            {
                if (!typeof(DbContext).IsAssignableFrom(type))
                {
                    throw new ArgumentException($"{type} must inherit from DbContext");
                }
            }
        }
    }
}

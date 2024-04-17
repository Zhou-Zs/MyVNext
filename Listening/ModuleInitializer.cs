using Commons;
using Microsoft.Extensions.DependencyInjection;

namespace Listening.Domain
{
    class ModuleInitializer : IModuleInitialiazer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<ListeningDomainService>();
        }
    }
}

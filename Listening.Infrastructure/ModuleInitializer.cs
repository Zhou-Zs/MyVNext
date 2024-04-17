using Commons;
using Listening.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Listening.Infrastructure
{
    class ModuleInitializer : IModuleInitialiazer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<IListeningRepository, ListeningRepository>();
        }
    }
}

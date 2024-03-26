using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons
{
    public interface IModuleInitialiazer
    {
        public void Initialize(IServiceCollection services);
    }
}

using Microsoft.Extensions.Configuration;

namespace AnyDBConfigProvider
{
    public class DBConfigurationSource : IConfigurationSource
    {
        private readonly DBConfigOptions _options;
        public DBConfigurationSource(DBConfigOptions options)
        {
            _options = options;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DBConfigurationProvider(_options);
        }
    }
}

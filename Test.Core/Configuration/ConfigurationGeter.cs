using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Core.Configuration
{
    public class ConfigurationGeter:IConfigurationGeter
    {
        private readonly IConfiguration _configuration;

        public ConfigurationGeter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string this[string key] => _configuration[key];

        public TConfig Get<TConfig>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Value cannot be null or wirtespace", nameof(key));
            }
            var section = _configuration.GetSection(key);
            return section.Get<TConfig>();
        }
    }
}

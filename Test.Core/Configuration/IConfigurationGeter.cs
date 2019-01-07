using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Core.Configuration
{
    public interface IConfigurationGeter
    {
        TConfig Get<TConfig>(string key);
        string this[string key] { get; }
    }
}

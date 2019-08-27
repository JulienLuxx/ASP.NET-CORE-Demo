using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Core.Map
{
    public interface IMapUtil
    {
        IDictionary<string, string> ObjectToDictionary(object obj);

        IDictionary<string, string> DynamicToDictionary(dynamic obj);

        List<string> DynamicToStringList(dynamic obj);
    }
}

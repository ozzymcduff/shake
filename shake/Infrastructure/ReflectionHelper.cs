using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace Shake.Infrastructure
{
    class ReflectionHelper
    {
        public static IDictionary<string, object> ObjectToDictionary(object o)
        {
            return o.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .ToDictionary(p => p.Name, p => p.GetValue(o, null), StringComparer.InvariantCultureIgnoreCase);
        }
    }
}

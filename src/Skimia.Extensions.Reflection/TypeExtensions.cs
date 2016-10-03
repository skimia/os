using System;
using System.Reflection;

namespace Skimia.Extensions.Reflection
{
    public static class TypeExtensions
    {
        public static bool HasInterface(this Type type, Type interfaceType)
        {
            return type.GetTypeInfo().FindInterfaces(new TypeFilter(FilterByName), interfaceType).Length > 0;
        }

        private static bool FilterByName(Type typeObj, object criteriaObj)
        {
            return typeObj.ToString() == criteriaObj.ToString();
        }
    }
}

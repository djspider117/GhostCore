using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GhostCore.Utility
{
    public static class AssemblyReflectionParser
    {
        public static IEnumerable<(Type Type, TAttrType Attribute)> GetTypesForAttribute<TAttrType>() where TAttrType : Attribute
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var typesInAssembly = assembly.GetTypes();
                var typesWithCustomAttribute = typesInAssembly.Where(x => x.IsDefined(typeof(TAttrType)));

                foreach (var type in typesWithCustomAttribute)
                {
                    var customAttribute = type.GetCustomAttribute<TAttrType>();
                    yield return (type, customAttribute);
                }
            }
        }

        public static IEnumerable<(Type Type, IEnumerable<TAttrType> Attributes)> GetTypesForAttributes<TAttrType>() where TAttrType : Attribute
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var typesInAssembly = assembly.GetTypes();
                var typesWithCustomAttribute = typesInAssembly.Where(x => x.IsDefined(typeof(TAttrType)));

                foreach (var type in typesWithCustomAttribute)
                {
                    var customAttributes = type.GetCustomAttributes<TAttrType>();
                    yield return (type, customAttributes);
                }
            }
        }
    }

}

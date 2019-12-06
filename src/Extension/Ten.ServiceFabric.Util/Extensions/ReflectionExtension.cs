using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Ten.ServiceFabric.Util.Extensions
{
    public static class ReflectionExtension
    {
        /// <summary>
        /// 获取当前项目程序集
        /// </summary>
        public static List<Assembly> GetAssemblies
        {
            get
            {
                var list = new List<Assembly>();
                var deps = DependencyContext.Default;
                //lib.Name.StartsWith("AspectCore")
                var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package");
                foreach (var lib in libs)
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    list.Add(assembly);
                }
                return list;
            }
        }

        /// <summary>
        /// 是否子类型继承父类型
        /// </summary>
        /// <param name="childType">子类型</param>
        /// <param name="parentType">父类型</param>
        /// <returns></returns>
        public static bool IsInheritType(this Type childType, Type parentType)
        {
            if (parentType.IsAssignableFrom(childType))
            {
                return true;
            }

            return false;
        }

    }
}
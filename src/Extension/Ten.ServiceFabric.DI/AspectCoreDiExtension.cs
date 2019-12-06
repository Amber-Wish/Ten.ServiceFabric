using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AspectCore.Injector;
using Ten.DDD.DomainCore.Injections;
using Ten.ServiceFabric.Util.Extensions;

namespace Ten.ServiceFabric.DI
{
    public static class AspectCoreDiExtension
    {
        public static void AddAspectCoreInject(this IServiceContainer builder, List<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var types = assembly.DefinedTypes;
                //扫描类
                var classTypes = types.Where(t => t.IsClass);
                foreach (var classType in classTypes)
                {
                    //扫描类实现的接口
                    var interfaceTypes = classType.ImplementedInterfaces.Where(
                        e => e.Name != typeof(ITransientDependency).Name
                             && e.Name != typeof(IScopedDependency).Name
                             && e.Name != typeof(ISingletonDependency).Name);
                    foreach (var interfaceType in interfaceTypes)
                    {
                        //注入 继承了 ITransientDependency接口的类
                        if (interfaceType.IsInheritType(typeof(ITransientDependency)))
                        {
                            builder.Add(new TypeServiceDefinition(interfaceType, classType, Lifetime.Transient));
                        }
                        //注入 继承了 IScopedDependency 接口的类
                        if (interfaceType.IsInheritType(typeof(IScopedDependency)))
                        {
                            builder.Add(new TypeServiceDefinition(interfaceType, classType, Lifetime.Scoped));
                        }
                        //注入 继承了 ISingletonDependency 接口的类
                        if (interfaceType.IsInheritType(typeof(ISingletonDependency)))
                        {
                            builder.Add(new TypeServiceDefinition(interfaceType, classType, Lifetime.Singleton));
                        }
                    }

                    // var type = Activator.CreateInstance(typeInfo);
                }
            }
        }
    }
}
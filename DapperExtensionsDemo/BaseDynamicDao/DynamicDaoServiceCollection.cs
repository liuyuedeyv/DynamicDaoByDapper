using Autofac;
using Autofac.Extras.DynamicProxy;
using DapperExtensionsDemo.Dao;
using System.Reflection;

namespace DapperExtensionsDemo.BaseDynamicDao
{
    public static class DynamicDaoServiceCollection
    {
        public static void AddDynamicDao(this ContainerBuilder containerBuilder, string dllFullpath)
        {
            containerBuilder.RegisterAssemblyTypes(Assembly.LoadFrom(dllFullpath)).AsImplementedInterfaces().SingleInstance()
                  .EnableInterfaceInterceptors().InterceptedBy(typeof(InterceptorDao));

            containerBuilder.RegisterType<InterceptorDao>();
        }
    }
}

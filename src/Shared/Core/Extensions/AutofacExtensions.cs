using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors.Castle;

namespace Core.Extensions
{
    public static class AutofacExtensions
    {
        public static void RegisterType<T, Y>(this ContainerBuilder builder)
        {
            builder.RegisterType<T>().As<Y>().EnableInterfaceInterceptors(new ProxyGenerationOptions()
            {
                Selector = new AspectInterceptorSelector()
            }).SingleInstance();
        }

        public static void RegisterController<T>(this ContainerBuilder builder)
        {
            builder.RegisterType<T>().EnableClassInterceptors(new ProxyGenerationOptions()
            {
                Selector = new AspectInterceptorSelector()
            });
        }
    }
}

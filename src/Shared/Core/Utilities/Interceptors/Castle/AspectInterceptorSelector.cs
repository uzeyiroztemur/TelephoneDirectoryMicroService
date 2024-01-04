using Castle.DynamicProxy;
using Core.Aspects.Autofac.Exception;
using Core.CrossCuttingConcerns.Logging;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Utilities.Interceptors.Castle
{
    public class AspectInterceptorSelector : IInterceptorSelector
    {
        private readonly ILogger _logger;
        public AspectInterceptorSelector()
        {
            _logger = ServiceTool.ServiceProvider.GetService<ILogger>();
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute>(true).ToList();
            var methodAttributes = type.GetMethods().Where(x => x.Name == method.Name && x.ReturnType == method.ReturnType).FirstOrDefault().GetCustomAttributes<MethodInterceptionBaseAttribute>(true);
            classAttributes.AddRange(methodAttributes);

            classAttributes.Add(new ExceptionLogAspect(_logger));

            return classAttributes.OrderBy(x => x.Priority).ToArray();
        }
    }
}

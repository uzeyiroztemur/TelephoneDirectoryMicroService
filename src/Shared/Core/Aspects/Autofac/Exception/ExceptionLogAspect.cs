using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Logging;
using Core.Utilities.Interceptors.Castle;

namespace Core.Aspects.Autofac.Exception
{
    public class ExceptionLogAspect : MethodInterception
    {
        private readonly ILogger _logger;

        public ExceptionLogAspect(ILogger logger)
        {
            _logger = logger;
        }

        protected override void OnException(IInvocation invocation, System.Exception ex)
        {
            _logger.HandleError(ex);
        }
    }
}

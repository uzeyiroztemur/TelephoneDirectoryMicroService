using Castle.DynamicProxy;
using Core.Utilities.Interceptors.Castle;
using System.Transactions;

namespace Core.Aspects.Autofac.Transaction
{
    public class TransactionScopeAspect : MethodInterception
    {
        public override void Intercept(IInvocation invocation)
        {
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    invocation.Proceed();
                    transactionScope.Complete();
                }
                catch (System.Exception ex)
                {
                    transactionScope.Dispose();
                    throw ex;
                }
            }
        }
    }

    public class TransactionScopeAspectAsync : MethodInterception
    {
        public override async void Intercept(IInvocation invocation)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    invocation.Proceed();

                    if (invocation.Method.ReturnType == typeof(Task))
                    {
                        var task = (Task)invocation.ReturnValue;
                        await task.ConfigureAwait(false);
                    }
                    else if (invocation.Method.ReturnType.IsGenericType && invocation.Method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                    {
                        var task = (Task)invocation.ReturnValue;
                        await task.ConfigureAwait(false);
                    }

                    transactionScope.Complete();
                }
                catch (System.Exception ex)
                {
                    transactionScope.Dispose();
                    throw ex;
                }
            }
        }
    }

}

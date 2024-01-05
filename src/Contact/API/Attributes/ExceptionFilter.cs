using Core.CrossCuttingConcerns.Logging;
using Core.Entities.DTOs;
using Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace API.Attributes
{
    public class ExceptionFilter : IActionFilter
    {
        private readonly Core.CrossCuttingConcerns.Logging.ILogger _logger;

        public ExceptionFilter(Core.CrossCuttingConcerns.Logging.ILogger logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception.NotNull())
            {
                _logger.HandleError(context.Exception);

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var result = new ErrorResult(context.Exception.Message);
                context.Result = new JsonResult(result);
                context.ExceptionHandled = true;
            }
        }
    }
}

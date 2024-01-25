using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Core.Extensions.Exception
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (System.Exception e)
            {

                await HandleExceptionAsync(httpContext, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, System.Exception e)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string message = "Internal Server Error";
            if (e.GetType() == typeof(ValidationException))
            {
                message = e.Message;
            }

            var errorDetails = new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = message,
            };

            return httpContext.Response.WriteAsync(errorDetails.ToString());
        }
    }
}

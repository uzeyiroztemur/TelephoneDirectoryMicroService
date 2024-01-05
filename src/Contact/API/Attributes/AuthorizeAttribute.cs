using Business.Constants;
using Core.Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace API.Attributes
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute() : base(typeof(AuthorizeFilter)) { }
    }

    public class AuthorizeFilter : IAuthorizationFilter
    {
        private readonly IConfiguration _configuration;

        public AuthorizeFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private bool AllowAnonymous(AuthorizationFilterContext context)
        {
            return context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute));
        }

        private string GetControllerName(AuthorizationFilterContext context) => ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
        private string GetActionName(AuthorizationFilterContext context) => ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (AllowAnonymous(context))
                return;

            //Api-Key Control
            var apiKey = context.HttpContext.Request.Headers["X-Api-Key"].ToString();
            if (!string.IsNullOrEmpty(apiKey) && !string.IsNullOrWhiteSpace(apiKey) && _configuration["AppOptions:ApiKey"] == apiKey)
            {
                return;
            }

            //Auth Control
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                //Authorization
                var controllerName = GetControllerName(context);
                var actionName = GetActionName(context);
            }
            else
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                var result = new ErrorResult(Messages.UnauthorizedRequest);
                context.Result = new JsonResult(result);
            }
            return;
        }
    }
}

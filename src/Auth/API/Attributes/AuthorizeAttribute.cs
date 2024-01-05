using Business.Abstract;
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
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthorizeFilter(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
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

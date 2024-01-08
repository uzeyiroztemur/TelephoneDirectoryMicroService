using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;

namespace API.Attributes
{
    public class LocalizationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var cultureKey = "tr-TR";
            if (!string.IsNullOrEmpty(context.HttpContext.Request.Headers["Language-Code"]))
            {
                if (context.HttpContext.Request.Headers["Language-Code"].ToString().ToUpper() == "EN")
                    cultureKey = "en-US";
            }

            if (DoesCultureExist(cultureKey))
            {
                var culture = new CultureInfo(cultureKey);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        private static bool DoesCultureExist(string cultureName)
        {
            return CultureInfo.GetCultures(CultureTypes.AllCultures).Any(culture => string.Equals(culture.Name, cultureName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}

using System.Globalization;

namespace API.Middleware
{
    public class LocalizationMiddleware
    {
        private readonly RequestDelegate _next;

        public LocalizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var cultureKey = "tr-TR";
            if (!string.IsNullOrEmpty(context.Request.Headers["Language-Code"]))
            {
                if (context.Request.Headers["Language-Code"].ToString().ToUpper() == "EN")
                    cultureKey = "en-US";
            }

            if (DoesCultureExist(cultureKey))
            {
                var culture = new CultureInfo(cultureKey);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }

            await _next(context);
        }

        private static bool DoesCultureExist(string cultureName)
        {
            return CultureInfo.GetCultures(CultureTypes.AllCultures).Any(culture => string.Equals(culture.Name, cultureName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}

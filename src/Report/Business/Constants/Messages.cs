using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Business.Constants
{
    public static class Messages
    {
        private static string GetLocalizeString(string key)
        {
            var _localizer = ServiceTool.ServiceProvider.GetService<IStringLocalizer>();
            return _localizer.GetString(key);
        }

        public static string UnauthorizedRequest => GetLocalizeString("UnauthorizedRequest");
        public static string RecordNotFound => GetLocalizeString("RecordNotFound");
    }
}

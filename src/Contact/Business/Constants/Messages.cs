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
        public static string FirstNameRequired => GetLocalizeString("FirstNameRequired");
        public static string FirstNameLength => GetLocalizeString("FirstNameLength");
        public static string LastNameRequired => GetLocalizeString("LastNameRequired");
        public static string LastNameLength => GetLocalizeString("LastNameLength");
        public static string CompanyLength => GetLocalizeString("CompanyLength");
        public static string InfoValueRequired => GetLocalizeString("InfoValueRequired");
        public static string InfoValueLength => GetLocalizeString("InfoValueLength");
        public static string PersonContactInfoHasBeenUsed => GetLocalizeString("PersonContactInfoHasBeenUsed");
        public static string PersonHasBeenUsed => GetLocalizeString("PersonHasBeenUsed");
    }
}

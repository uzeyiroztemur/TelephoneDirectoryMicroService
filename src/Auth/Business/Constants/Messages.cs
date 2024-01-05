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

        public static string PasswordMatch => GetLocalizeString("PasswordMatch");
        public static string UserNotFound => GetLocalizeString("UserNotFound");
        public static string PasswordDoesNotMatch => GetLocalizeString("PasswordDoesNotMatch");
        public static string PasswordDoesNotLength => GetLocalizeString("PasswordDoesNotLength");
        public static string UserPasswordChanged => GetLocalizeString("UserPasswordChanged");
        public static string PasswordInvalid => GetLocalizeString("PasswordInvalid");
        public static string UserMustChangePassword => GetLocalizeString("UserMustChangePassword");
        public static string UserAgentParseError => GetLocalizeString("UserAgentParseError");
        public static string UserDeviceNotRegistered => GetLocalizeString("UserDeviceNotRegistered");
        public static string UserSuccessfullLogin => GetLocalizeString("UserSuccessfullLogin");
        public static string UserAccessTokenCreated => GetLocalizeString("UserAccessTokenCreated");
        public static string PasswordUndefined => GetLocalizeString("PasswordUndefined");
        public static string UserAccountLockeddown => GetLocalizeString("UserAccountLockeddown");
        public static string PasswordRequired => GetLocalizeString("PasswordRequired");
        public static string PasswordLength => GetLocalizeString("PasswordLength");
        public static string RecordNotFound => GetLocalizeString("RecordNotFound");
        public static string UserAgentNotFound => GetLocalizeString("UserAgentNotFound");
        public static string UserNameRequired => GetLocalizeString("UserNameRequired");
        public static string UnauthorizedRequest => GetLocalizeString("UnauthorizedRequest");
    }
}

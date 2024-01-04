namespace Core.Extensions
{
    public static class UrlExtensions
    {
        public static string FormatUrl(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            return !str.StartsWith("http://") && !str.StartsWith("https://") ? string.Format("http://{0}", (object)str) : str;
        }
    }
}

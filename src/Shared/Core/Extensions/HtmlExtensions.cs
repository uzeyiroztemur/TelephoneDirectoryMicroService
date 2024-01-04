
using System.Text.RegularExpressions;
using System.Web;

namespace Core.Extensions
{
    public static class HtmlExtensions
    {
        public static string ClearTags(this string Value, bool Quotes = false)
        {
            Value = Value.HtmlDecode();
            if (!Value.NotNull<string>())
                return string.Empty;
            if (Quotes)
                Value = Value.Replace("\"", "-").Replace("'", "-");
            return Regex.Replace(Value, "<[^>]*>", string.Empty);
        }

        public static string ClearScriptTags(this string Value)
        {
            Value = Value.HtmlDecode();
            return !Value.NotNull<string>() ? string.Empty : Regex.Replace(Value, "<script.*?>.*?</script>", string.Empty);
        }

        public static string ClearStyleTags(this string Value)
        {
            Value = Value.HtmlDecode();
            return !Value.NotNull<string>() ? string.Empty : Regex.Replace(Value, "(<style.*?>.*?</style>)", string.Empty);
        }

        public static string ClearNoScriptTags(this string Value)
        {
            Value = Value.HtmlDecode();
            return !Value.NotNull<string>() ? string.Empty : Regex.Replace(Value, "(<noscript.*?>.*?</noscript>)", string.Empty);
        }

        public static string HtmlDecode(this string Value) => HttpUtility.HtmlDecode(Value);

        public static string HtmlEncode(this string Value) => HttpUtility.HtmlEncode(Value);

        public static string UrlEncode(this string Value) => HttpUtility.UrlEncode(Value);

        public static string UrlDecode(this string Value) => HttpUtility.UrlDecode(Value);

        public static string ReplaceQueryString(this string Url, string Key, string Value)
        {
            string pattern = string.Format("[(\\&)|(\\?)]{0}(=[^&\\\\?]+)", (object)Key);
            string input = Regex.Replace(Url, pattern, "{0}");
            if (Key.ToLowerInvariant() != "pg")
                input = Regex.Replace(input, "[(\\&)|(\\?)]pg(=[^&\\\\?]+)", string.Empty);
            string str = Regex.Replace(input, "(.aspx\\&)", ".aspx?");
            if (Value.Length > 0)
                str = string.Format(str, (object)string.Format("{0}{1}={2}", Regex.IsMatch(str, "(\\?)") ? (object)"&" : (object)"?", (object)Key, (object)Value));
            if (str.Contains("?") && !str.Contains(Key))
                str += string.Format("&{0}={1}", (object)Key, (object)Value);
            if (!str.Contains("?") && !str.Contains(Key))
                str += string.Format("?{0}={1}", (object)Key, (object)Value);
            return str;
        }

        public static string DeleteQueryString(this string Url, string Key)
        {
            string input = Regex.Replace(Url, string.Format("[(\\&)|(\\?)]{0}(=[^&\\\\?]+)", (object)Key), string.Empty);
            if (Key.ToLowerInvariant() != "pg")
                input = Regex.Replace(input, "[(\\&)|(\\?)]pg(=[^&\\\\?]+)", string.Empty);
            string str = Regex.Replace(input, "(.aspx\\&)", ".aspx?");
            if (!str.Contains("?"))
            {
                int startIndex = str.IndexOf("&");
                if (startIndex != -1)
                    str = str.Remove(startIndex, 1).Insert(startIndex, "?");
            }
            return str;
        }
    }
}

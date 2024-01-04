using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string Value)
        {
            return Value.IsNull<string>() || string.IsNullOrEmpty(Value);
        }

        public static bool NotEmpty(this string Value)
        {
            return Value.NotNull<string>() && !string.IsNullOrEmpty(Value);
        }

        public static string Left(this string Value, int Length)
        {
            if (!Value.NotEmpty())
                return string.Empty;
            return Value.Length <= Length ? Value : Value.Substring(0, Length);
        }

        public static string Left(this string Value, int Length, string Suffix)
        {
            if (!Value.NotEmpty())
                return Suffix;
            return Value.Length <= Length ? Value : string.Format("{0}{1}", (object)Value.Substring(0, Length), (object)Suffix);
        }

        public static string Format(this string Value, params object[] Args)
        {
            return string.Format(Value, Args);
        }

        public static string ToLowerTurkish(this string Value)
        {
            return Value.ToLower(new CultureInfo("tr-TR", false));
        }

        public static string ToLowerEnglish(this string Value)
        {
            string lowerEnglish = string.Empty;
            if (Value.NotNull<string>())
                lowerEnglish = Value.Replace("ı", "i").Replace("İ", "i").Replace("I", "i").Replace("ğ", "g").Replace("Ğ", "g").Replace("ü", "u").Replace("Ü", "u").Replace("ş", "s").Replace("Ş", "s").Replace("ö", "o").Replace("Ö", "o").Replace("ç", "c").Replace("Ç", "c").ToLower(new CultureInfo("en-US", false));
            return lowerEnglish;
        }

        public static string ToUpperEnglish(this string Value)
        {
            string upperEnglish = string.Empty;
            if (Value.NotNull<string>())
                upperEnglish = Value.Replace("I", "i").Replace("İ", "i").Replace("ı", "i").Replace("Ğ", "g").Replace("ğ", "g").Replace("Ü", "u").Replace("ü", "u").Replace("Ş", "s").Replace("ş", "s").Replace("Ç", "c").Replace("ç", "c").Replace("Ö", "o").Replace("ö", "o").ToUpperInvariant();
            return upperEnglish;
        }

        public static string UrlMaker(this string Value)
        {
            string str = string.Empty;
            if (Value.NotNull<string>())
                str = Regex.Replace(Value.Replace(" - ", "-").Trim().Replace(" ", "-").Replace(".", string.Empty).Replace("\"", string.Empty).Replace("'", string.Empty).Replace("‘", string.Empty).Replace("’", string.Empty).Replace("“", string.Empty).Replace("”", string.Empty).Replace("!", string.Empty).Replace("^", string.Empty).Replace("+", string.Empty).Replace("&", string.Empty).Replace("/", string.Empty).Replace("\\", string.Empty).Replace("{", string.Empty).Replace("}", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).Replace("?", string.Empty).Replace("=", string.Empty).Replace("*", string.Empty).Replace("_", "-").Replace("|", "-").Replace("<", string.Empty).Replace(">", string.Empty).ToLowerEnglish(), "[^\\w\\.@-]", string.Empty);
            return str;
        }

        public static string UrlMaker(this string Value, IList<string> Values)
        {
            string str1 = Value.UrlMaker();
            if (!Values.Contains(str1))
                return str1;
            int num = 1;
            string str2;
            do
            {
                str2 = string.Format("{0}-{1}", (object)str1, (object)num++);
            }
            while (Values.Contains(str2));
            return str2;
        }

        public static Guid ToGuid(this string Value)
        {
            Guid guid = new Guid("00000000-0000-0000-0000-000000000000");
            try
            {
                guid = new Guid(Value);
            }
            catch
            {
            }
            return guid;
        }
    }
}

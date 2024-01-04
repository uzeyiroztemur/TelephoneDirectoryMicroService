using System.Globalization;

namespace Core.Extensions
{
    public static class DateExtensions
    {
        public static DateTime ToDateTime(this string Value)
        {
            return DateExtensions.ToDateTime(Value, DateTime.MinValue, DateTime.MaxValue);
        }

        public static DateTime ToDateTime(this string Value, DateTime Min)
        {
            return DateExtensions.ToDateTime(Value, Min, DateTime.MaxValue);
        }

        public static DateTime ToDateTime(this string Value, DateTime Min, DateTime Max)
        {
            DateTime result;
            if (DateTime.TryParse(Value, out result))
            {
                if (result < Min)
                    result = Min;
                else if (result > Max)
                    result = Max;
            }
            return result;
        }

        public static DateTime ToDateTime(this object Value)
        {
            return Value.ToDateTime(DateTime.MinValue, DateTime.MaxValue);
        }

        public static DateTime ToDateTime(this object Value, DateTime Min)
        {
            return Value.ToDateTime(Min, DateTime.MaxValue);
        }

        public static DateTime ToDateTime(this object Value, DateTime Min, DateTime Max)
        {
            DateTime result = Min;
            if (Value.NotNull<object>() && DateTime.TryParse(Value.ToString(), out result))
            {
                if (result < Min)
                    result = Min;
                else if (result > Max)
                    result = Max;
            }
            return result;
        }

        public static DateTime ToDateTime(this string Value, string Format)
        {
            return DateExtensions.ToDateTime(Value, Format, DateTime.MinValue, DateTime.MaxValue);
        }

        public static DateTime ToDateTime(this string Value, string Format, DateTime Min)
        {
            return DateExtensions.ToDateTime(Value, Format, Min, DateTime.MaxValue);
        }

        public static DateTime ToDateTime(
          this string Value,
          string Format,
          DateTime Min,
          DateTime Max)
        {
            DateTime result;
            if (DateTime.TryParseExact(Value, Format, (IFormatProvider)CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                if (result < Min)
                    result = Min;
                else if (result > Max)
                    result = Max;
            }
            return result;
        }

        public static DateTime ToDateTime(this object Value, string Format)
        {
            return Value.ToDateTime(Format, DateTime.MinValue, DateTime.MaxValue);
        }

        public static DateTime ToDateTime(this object Value, string Format, DateTime Min)
        {
            return Value.ToDateTime(Format, Min, DateTime.MaxValue);
        }

        public static DateTime ToDateTime(
          this object Value,
          string Format,
          DateTime Min,
          DateTime Max)
        {
            DateTime result = Min;
            if (Value.NotNull<object>() && DateTime.TryParseExact(Value.ToString(), Format, (IFormatProvider)CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                if (result < Min)
                    result = Min;
                else if (result > Max)
                    result = Max;
            }
            return result;
        }

        public static string FormatDate(this string Value)
        {
            return DateExtensions.ToDateTime(Value).FormatDate();
        }

        public static string FormatDate(this object Value) => Value.ToDateTime().FormatDate();

        public static string FormatDate(this DateTime Value)
        {
            string format = "{0} {1}";
            DateTime now = DateTime.Now;
            if (Value <= now.AddDays(-7.0))
                format = string.Format(format, (object)Value.ToString("dd MMM yy, HH:mm"), (object)string.Empty);
            else if (Value >= now.AddSeconds(-60.0))
                format = string.Format(format, (object)Math.Abs((now - Value).Seconds), (object)"saniye önce");
            else if (Value >= now.AddMinutes(-60.0))
                format = string.Format(format, (object)Math.Abs((now - Value).Minutes), (object)"dakika önce");
            else if (Value >= now.AddHours(-24.0))
                format = string.Format(format, (object)Math.Abs((now - Value).Hours), (object)"saat önce");
            else if (Value >= now.AddDays(-7.0))
                format = string.Format(format, (object)Math.Abs((now - Value).Days), (object)"gün önce");
            return format.Trim();
        }

        public static int DiffMilliseconds(this DateTime Greater, DateTime Lower)
        {
            return (int)(Greater - Lower).TotalMilliseconds;
        }

        public static int DiffSeconds(this DateTime Greater, DateTime Lower)
        {
            return (int)(Greater - Lower).TotalSeconds;
        }

        public static int DiffMinutes(this DateTime Greater, DateTime Lower)
        {
            return (int)(Greater - Lower).TotalMinutes;
        }

        public static int DiffHours(this DateTime Greater, DateTime Lower)
        {
            return (int)(Greater - Lower).TotalHours;
        }

        public static int DiffDays(this DateTime Greater, DateTime Lower)
        {
            return (int)(Greater - Lower).TotalDays;
        }

        public static bool IsDate(this object val)
        {
            if (!val.IsDateTime())
                return false;
            DateTime result;
            DateTime.TryParse(val.ToString(), out result);
            return result.StartOfDay() == result;
        }

        public static bool IsDateTime(this object val)
        {
            if (val.GetType() == typeof(DateTime) || val.GetType() == typeof(DateTime?))
                return true;
            if (val == null || val.ToString().IsEmpty())
                return false;
            DateTime.TryParse(val.ToString(), out DateTime _);
            return true;
        }

        public static DateTime StartOfDay(this DateTime theDate) => theDate.Date;

        public static DateTime EndOfDay(this DateTime theDate)
        {
            DateTime dateTime = theDate.Date;
            dateTime = dateTime.AddDays(1.0);
            return dateTime.AddTicks(-1L);
        }
    }
}

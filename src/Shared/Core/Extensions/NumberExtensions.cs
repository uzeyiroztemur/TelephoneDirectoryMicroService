using System;

namespace Core.Extensions
{
    public static class NumberExtensions
    {
        public static int ToInteger(this string Value)
        {
            return NumberExtensions.ToInteger(Value, int.MinValue, int.MaxValue);
        }

        public static int ToInteger(this string Value, int Min)
        {
            return NumberExtensions.ToInteger(Value, Min, int.MaxValue);
        }

        public static int ToInteger(this string Value, int Min, int Max)
        {
            int result;
            if (int.TryParse(Value, out result))
            {
                if (result < Min)
                    result = Min;
                else if (result > Max)
                    result = Max;
            }
            return result;
        }

        public static int ToInteger(this object Value) => Value.ToInteger(int.MinValue, int.MaxValue);

        public static int ToInteger(this object Value, int Min) => Value.ToInteger(Min, int.MaxValue);

        public static int ToInteger(this object Value, int Min, int Max)
        {
            int result = Min;
            if (Value.NotNull<object>() && int.TryParse(Value.ToString(), out result))
            {
                if (result < Min)
                    result = Min;
                else if (result > Max)
                    result = Max;
            }
            return result;
        }

        public static double ToDouble(this string Value)
        {
            return NumberExtensions.ToDouble(Value, double.MinValue, double.MaxValue);
        }

        public static double ToDouble(this string Value, double Min)
        {
            return NumberExtensions.ToDouble(Value, Min, double.MaxValue);
        }

        public static double ToDouble(this string Value, double Min, double Max)
        {
            double result;
            if (double.TryParse(Value, out result))
            {
                if (result < Min)
                    result = Min;
                else if (result > Max)
                    result = Max;
            }
            return result;
        }

        public static double ToDouble(this object Value)
        {
            return Value.ToDouble(double.MinValue, double.MaxValue);
        }

        public static double ToDouble(this object Value, double Min)
        {
            return Value.ToDouble(Min, double.MaxValue);
        }

        public static double ToDouble(this object Value, double Min, double Max)
        {
            double result = Min;
            if (Value.NotNull<object>() && double.TryParse(Value.ToString(), out result))
            {
                if (result < Min)
                    result = Min;
                else if (result > Max)
                    result = Max;
            }
            return result;
        }

        public static float ToFloat(this string Value)
        {
            return NumberExtensions.ToFloat(Value, float.MinValue, float.MaxValue);
        }

        public static float ToFloat(this string Value, float Min)
        {
            return NumberExtensions.ToFloat(Value, Min, float.MaxValue);
        }

        public static float ToFloat(this string Value, float Min, float Max)
        {
            float result;
            if (float.TryParse(Value, out result))
            {
                if ((double)result < (double)Min)
                    result = Min;
                else if ((double)result > (double)Max)
                    result = Max;
            }
            return result;
        }

        public static float ToFloat(this object Value) => Value.ToFloat(float.MinValue, float.MaxValue);

        public static float ToFloat(this object Value, float Min) => Value.ToFloat(Min, float.MaxValue);

        public static float ToFloat(this object Value, float Min, float Max)
        {
            float result = Min;
            if (Value.NotNull<object>() && float.TryParse(Value.ToString(), out result))
            {
                if ((double)result < (double)Min)
                    result = Min;
                else if ((double)result > (double)Max)
                    result = Max;
            }
            return result;
        }

        public static Decimal ToDecimal(this string Value)
        {
            return NumberExtensions.ToDecimal(Value, Decimal.MinValue, Decimal.MaxValue);
        }

        public static Decimal ToDecimal(this string Value, Decimal Min)
        {
            return NumberExtensions.ToDecimal(Value, Min, Decimal.MaxValue);
        }

        public static Decimal ToDecimal(this string Value, Decimal Min, Decimal Max)
        {
            Decimal result;
            if (Decimal.TryParse(Value, out result))
            {
                if (result < Min)
                    result = Min;
                else if (result > Max)
                    result = Max;
            }
            return result;
        }

        public static Decimal ToDecimal(this object Value)
        {
            return Value.ToDecimal(Decimal.MinValue, Decimal.MaxValue);
        }

        public static Decimal ToDecimal(this object Value, Decimal Min)
        {
            return Value.ToDecimal(Min, Decimal.MaxValue);
        }

        public static Decimal ToDecimal(this object Value, Decimal Min, Decimal Max)
        {
            Decimal result = Min;
            if (Value.NotNull<object>() && Decimal.TryParse(Value.ToString(), out result))
            {
                if (result < Min)
                    result = Min;
                else if (result > Max)
                    result = Max;
            }
            return result;
        }

        public static string FormatInteger(this string Value)
        {
            return NumberExtensions.ToInteger(Value).FormatInteger();
        }

        public static string FormatInteger(this object Value) => Value.ToInteger().FormatInteger();

        public static string FormatInteger(this int Value)
        {
            string format = "{0} {1}";
            if (Value < 1000)
                format = string.Format(format, (object)Value, (object)string.Empty);
            else if (Value > 999999)
                format = string.Format(format, (object)(int)Math.Floor((double)Value / 1000000.0), (object)"milyon");
            else if (Value > 999)
                format = string.Format(format, (object)(int)Math.Floor((double)Value / 1000.0), (object)"bin");
            return format;
        }
    }
}

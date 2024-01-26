using System;

namespace Core.Extensions
{
    public static class TypeExtensions
    {
        public static object TryTypeConvert(this object p, Type type)
        {
            if (type == typeof(bool))
                return (object)(p.ToString() == "True");
            if (type == typeof(decimal?) && p.GetType() == typeof(string))
            {
                return (object)(decimal.TryParse(p.ToString(), out decimal result) ? new Decimal?(result) : new Decimal?());
            }
            if (type == typeof(decimal) && p.GetType() == typeof(string))
                return (object)Decimal.Parse(p.ToString());
            if (type == typeof(short) && p.GetType() == typeof(string))
                return (object)short.Parse(p.ToString());
            if (type == typeof(short?) && p.GetType() == typeof(string))
            {
                int result;
                return (object)(int.TryParse(p.ToString(), out result) ? new int?(result) : new int?());
            }
            if (type == typeof(int) && p.GetType() == typeof(string))
                return (object)int.Parse(p.ToString());
            if (type == typeof(int?) && p.GetType() == typeof(string))
            {
                int result;
                return (object)(int.TryParse(p.ToString(), out result) ? new int?(result) : new int?());
            }
            if (type == typeof(long) && p.GetType() == typeof(string))
                return (object)long.Parse(p.ToString());
            if (type == typeof(byte) && p.GetType() == typeof(string))
                return (object)byte.Parse(p.ToString());
            if (type == typeof(DateTime?) && p.GetType() == typeof(string))
            {
                DateTime result;
                return (object)(DateTime.TryParse(p.ToString(), out result) ? new DateTime?(result) : new DateTime?());
            }
            if (type == typeof(DateTime?) && p.GetType() == typeof(DateTime))
            {
                DateTime result;
                return (object)(DateTime.TryParse(p.ToString(), out result) ? new DateTime?(result) : new DateTime?());
            }
            if (type == typeof(DateTime) && p.GetType() == typeof(string))
                return (object)DateTime.Parse(p.ToString());
            return type.IsEnum ? Enum.Parse(type, p.ToString()) : p;
        }

        public static object GetDefault(this Type type)
        {
            return type.NotNull<Type>() && type.IsValueType ? Activator.CreateInstance(type) : (object)null;
        }
    }
}

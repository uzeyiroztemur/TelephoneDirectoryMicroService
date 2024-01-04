using Core.Entities.DTOs;
using Core.Extensions;
using Core.Utilities.Filtering.Parameters;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Utilities.Filtering
{
    public static class FilterManager
    {
        private static readonly MethodInfo ContainsMethod = typeof(string).GetMethod("Contains", new Type[1]
        {
      typeof (string)
        });
        private static readonly MethodInfo StartsWithMethod = typeof(string).GetMethod("StartsWith", new Type[1]
        {
      typeof (string)
        });
        private static readonly MethodInfo EndsWithMethod = typeof(string).GetMethod("EndsWith", new Type[1]
        {
      typeof (string)
        });
        private static readonly MethodInfo ToStringMethod = typeof(object).GetMethod("ToString");

        private static IQueryable<T> GetFilter<T>(this IQueryable<T> query, Criter prms)
        {
            Expression<Func<T, bool>> filter = FilterManager.GetFilter<T>(prms);
            if (prms != null && prms.Sort != null && prms.Sort.Count > 0 && !string.IsNullOrEmpty(prms.Sort.First<Core.Utilities.Filtering.Parameters.Sort>().Field))
                query = query.OrderBy<T>(prms.Sort.First<Core.Utilities.Filtering.Parameters.Sort>().Field, prms.Sort.First<Core.Utilities.Filtering.Parameters.Sort>().Dir);
            return filter != null ? query.Where<T>(filter) : query;
        }

        private static IQueryable<T> ToPagedList<T>(this IQueryable<T> query, Criter prms)
        {
            IQueryable<T> source1 = query;
            int? nullable;
            int count1;
            if (!prms.Skip.HasValue)
            {
                count1 = 0;
            }
            else
            {
                nullable = prms.Skip;
                count1 = nullable.Value;
            }
            IQueryable<T> source2 = Queryable.Skip<T>(source1, count1);
            nullable = prms.Take;
            int count2;
            if (!nullable.HasValue)
            {
                count2 = 10;
            }
            else
            {
                nullable = prms.Take;
                count2 = nullable.Value;
            }
            return Queryable.Take<T>(source2, count2);
        }

        public static IDataResult<IList<T>> ToFilteredList<T>(this IQueryable<T> query, Criter prms)
        {
            try
            {
                IQueryable<T> filter = query.GetFilter<T>(prms);
                int count = Queryable.Count<T>(query);
                Criter prms1 = prms;
                return (IDataResult<IList<T>>)new SuccessDataResult<IList<T>>((IList<T>)filter.ToPagedList<T>(prms1).ToList<T>(), count);
            }
            catch (Exception ex)
            {
                return (IDataResult<IList<T>>)new ErrorDataResult<IList<T>>(ex.Message);
            }
        }

        public static IDataResult<IList<T>> ToFilteredList<T>(this IQueryable<T> query)
        {
            try
            {
                List<T> list = query.ToList<T>();
                return (IDataResult<IList<T>>)new SuccessDataResult<IList<T>>((IList<T>)list, list.Count);
            }
            catch (Exception ex)
            {
                return (IDataResult<IList<T>>)new ErrorDataResult<IList<T>>(ex.Message);
            }
        }

        public static IDataResult<T> GetFirst<T>(this IQueryable<T> query)
        {
            try
            {
                return (IDataResult<T>)new SuccessDataResult<T>(Queryable.FirstOrDefault<T>(query));
            }
            catch (Exception ex)
            {
                return (IDataResult<T>)new ErrorDataResult<T>(ex.Message);
            }
        }

        private static Expression<Func<T, bool>> GetFilter<T>(Criter prms)
        {
            return prms == null || prms.Filter == null || prms.Filter.Filters.Count <= 0 ? (Expression<Func<T, bool>>)null : FilterManager.GetFilter<T>(FilterManager.GetFilterList<T>(prms.Filter.Filters), prms.Filter.Logic);
        }

        private static List<FilterArg> GetFilterList<T>(List<FilterItem> filterItems)
        {
            List<FilterArg> filters = new List<FilterArg>();
            foreach (FilterItem filterItem in filterItems)
            {
                if (filterItem.Filters != null && filterItem.Filters.Count > 0)
                {
                    foreach (FilterArg filter in FilterManager.GetFilterList<T>(filterItem.Filters))
                        filters.Add(filter);
                }
                else
                {
                    FilterArg filter = new FilterArg()
                    {
                        Operation = filterItem.Operator,
                        PropertyName = filterItem.Field,
                        Value = (object)filterItem.Value
                    };
                    MemberExpression memberExpression = Expression.Property((Expression)Expression.Parameter(typeof(T), "t"), filter.PropertyName);
                    if ((memberExpression.Type == typeof(DateTime) || memberExpression.Type == typeof(DateTime?)) && filter.Value != null && filter.Value.IsDate())
                        FilterManager.AddDateFilters(filters, filter);
                    else
                        filters.Add(filter);
                }
            }
            return filters;
        }

        private static Expression<Func<T, bool>> GetFilter<T>(List<FilterArg> prms, Logic logic)
        {
            return FilterManager.GetExpression<T>((IList<FilterArg>)prms, logic);
        }

        private static Expression<Func<T, bool>> GetExpression<T>(IList<FilterArg> filters, Logic logic)
        {
            if (filters.Count == 0)
                return (Expression<Func<T, bool>>)null;
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "t");
            Expression expression = (Expression)null;
            if (filters.Count == 1)
                expression = FilterManager.GetExpression<T>(parameterExpression, filters[0]);
            else if (filters.Count == 2)
            {
                expression = (Expression)FilterManager.GetExpression<T>(parameterExpression, filters[0], filters[1], logic);
            }
            else
            {
                while (filters.Count > 0)
                {
                    FilterArg filter1 = filters[0];
                    FilterArg filter2 = filters[1];
                    expression = expression != null ? (logic != Logic.And ? (Expression)Expression.OrElse(expression, (Expression)FilterManager.GetExpression<T>(parameterExpression, filters[0], filters[1], logic)) : (Expression)Expression.AndAlso(expression, (Expression)FilterManager.GetExpression<T>(parameterExpression, filters[0], filters[1], logic))) : (Expression)FilterManager.GetExpression<T>(parameterExpression, filters[0], filters[1], logic);
                    filters.Remove(filter1);
                    filters.Remove(filter2);
                    if (filters.Count == 1)
                    {
                        expression = logic != Logic.And ? (Expression)Expression.OrElse(expression, FilterManager.GetExpression<T>(parameterExpression, filters[0])) : (Expression)Expression.AndAlso(expression, FilterManager.GetExpression<T>(parameterExpression, filters[0]));
                        filters.RemoveAt(0);
                    }
                }
            }
            return Expression.Lambda<Func<T, bool>>(expression, parameterExpression);
        }

        private static BinaryExpression GetExpression<T>(
          ParameterExpression param,
          FilterArg filter1,
          FilterArg filter2,
          Logic logic)
        {
            return logic == Logic.And ? Expression.AndAlso(FilterManager.GetExpression<T>(param, filter1), FilterManager.GetExpression<T>(param, filter2)) : Expression.OrElse(FilterManager.GetExpression<T>(param, filter1), FilterManager.GetExpression<T>(param, filter2));
        }

        private static Expression GetExpression<T>(ParameterExpression param, FilterArg filter)
        {
            MemberExpression e1 = Expression.Property((Expression)param, filter.PropertyName);
            if (filter.Value != null && filter.Value.GetType() != e1.Type && filter.Operation != Operation.Contains)
                filter.Value = filter.Value.TryTypeConvert(e1.Type);
            Expression e2 = (Expression)param;
            if (filter.Value != null)
                e2 = (Expression)Expression.Constant(filter.Value);
            return FilterManager.GetExpression((Expression)e1, e2, filter.Operation);
        }

        private static Expression GetExpression(Expression e1, Expression e2, Operation op)
        {
            if (op == Operation.IsNotNull || op == Operation.IsNull)
                e2 = (Expression)Expression.Constant((object)null, typeof(object));
            else if (FilterManager.IsNullableType(e1.Type) && !FilterManager.IsNullableType(e2.Type))
                e2 = (Expression)Expression.Convert(e2, e1.Type);
            else if (!FilterManager.IsNullableType(e1.Type) && FilterManager.IsNullableType(e2.Type))
                e1 = (Expression)Expression.Convert(e1, e2.Type);
            switch (op)
            {
                case Operation.Equals:
                    return (Expression)Expression.Equal(e1, e2);
                case Operation.GreaterThan:
                    return (Expression)Expression.GreaterThan(e1, e2);
                case Operation.LessThan:
                    return (Expression)Expression.LessThan(e1, e2);
                case Operation.GreaterThanOrEqual:
                    return (Expression)Expression.GreaterThanOrEqual(e1, e2);
                case Operation.LessThanOrEqual:
                    return (Expression)Expression.LessThanOrEqual(e1, e2);
                case Operation.Contains:
                    return e1.Type != typeof(string) ? (Expression)Expression.Call((Expression)Expression.Call(e1, FilterManager.ToStringMethod), FilterManager.ContainsMethod, e2) : (Expression)Expression.Call(e1, FilterManager.ContainsMethod, e2);
                case Operation.StartsWith:
                    return (Expression)Expression.Call(e1, FilterManager.StartsWithMethod, e2);
                case Operation.EndsWith:
                    return (Expression)Expression.Call(e1, FilterManager.EndsWithMethod, e2);
                case Operation.NotEquals:
                    return (Expression)Expression.NotEqual(e1, e2);
                case Operation.NotContains:
                    return (Expression)Expression.Not((Expression)Expression.Call(e1, FilterManager.ContainsMethod, e2));
                case Operation.IsNotNull:
                    return (Expression)Expression.NotEqual(e1, e2);
                case Operation.IsNull:
                    return (Expression)Expression.Equal(e1, e2);
                default:
                    return (Expression)null;
            }
        }

        private static bool IsNullableType(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static IQueryable<T> OrderBy<T>(
          this IQueryable<T> q,
          string sort,
          SortingDirection sortType)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "t");
            PropertyInfo property = typeof(T).GetProperty(sort);
            q = q.Provider.CreateQuery<T>((Expression)Expression.Call(typeof(Queryable), sortType == SortingDirection.Asc ? nameof(OrderBy) : "OrderByDescending", new Type[2]
            {
        typeof (T),
        property.PropertyType
            }, q.Expression, (Expression)Expression.Lambda((Expression)Expression.Property((Expression)parameterExpression, property), parameterExpression)));
            return q;
        }

        private static void AddDateFilters(List<FilterArg> filters, FilterArg filter)
        {
            DateTime result;
            DateTime.TryParse(filter.Value.ToString(), out result);
            if (filter.Operation == Operation.Equals)
            {
                filters.Add(new FilterArg()
                {
                    Operation = Operation.GreaterThan,
                    PropertyName = filter.PropertyName,
                    Value = (object)result.StartOfDay().ToString()
                });
                filters.Add(new FilterArg()
                {
                    Operation = Operation.LessThan,
                    PropertyName = filter.PropertyName,
                    Value = (object)result.EndOfDay().ToString()
                });
            }
            else if (filter.Operation == Operation.GreaterThan || filter.Operation == Operation.LessThanOrEqual)
                filters.Add(new FilterArg()
                {
                    Operation = filter.Operation,
                    PropertyName = filter.PropertyName,
                    Value = (object)result.EndOfDay().ToString()
                });
            else
                filters.Add(filter);
        }
    }
}

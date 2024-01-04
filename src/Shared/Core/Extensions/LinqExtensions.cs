namespace Core.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> Traverse<T>(this T item, Func<T, T> childSelector)
        {
            Stack<T> stack = new Stack<T>((IEnumerable<T>)new T[1]
            {
        item
            });
            while (stack.Any<T>())
            {
                T next = stack.Pop();
                if ((object)next != null)
                {
                    yield return next;
                    stack.Push(childSelector(next));
                }
                next = default(T);
            }
        }

        public static IEnumerable<T> Traverse<T>(this T item, Func<T, IEnumerable<T>> childSelector)
        {
            Stack<T> stack = new Stack<T>((IEnumerable<T>)new T[1]
            {
        item
            });
            while (stack.Any<T>())
            {
                T next = stack.Pop();
                yield return next;
                foreach (T obj in childSelector(next))
                    stack.Push(obj);
                next = default(T);
            }
        }

        public static IEnumerable<T> Traverse<T>(
          this IEnumerable<T> items,
          Func<T, IEnumerable<T>> childSelector)
        {
            Stack<T> stack = new Stack<T>(items);
            while (stack.Any<T>())
            {
                T next = stack.Pop();
                yield return next;
                foreach (T obj in childSelector(next))
                    stack.Push(obj);
                next = default(T);
            }
        }

        public static IEnumerable<Tuple<T1, T2>> CrossJoin<T1, T2>(
          this IEnumerable<T1> sequence1,
          IEnumerable<T2> sequence2)
        {
            return sequence1.SelectMany<T1, Tuple<T1, T2>>((Func<T1, IEnumerable<Tuple<T1, T2>>>)(t1 => sequence2.Select<T2, Tuple<T1, T2>>((Func<T2, Tuple<T1, T2>>)(t2 => Tuple.Create<T1, T2>(t1, t2)))));
        }
    }
}

namespace Core.Extensions
{
  public static class CommonExtensions
  {
    public static bool IsNull<T>(this T item) => (object) item == null;

    public static bool NotNull<T>(this T item) => (object) item != null;
  }
}

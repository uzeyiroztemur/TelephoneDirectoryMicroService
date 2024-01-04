namespace Core.Entities.DTOs
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        public ErrorDataResult(T data, int count, string message)
          : base(data, count, false, message)
        {
        }

        public ErrorDataResult(T data, string message)
          : base(data, false, message)
        {
        }

        public ErrorDataResult(T data, int count)
          : base(data, count, false)
        {
        }

        public ErrorDataResult(T data)
          : base(data, false)
        {
        }

        public ErrorDataResult(string message)
          : base(default(T), false, message)
        {
        }

        public ErrorDataResult()
          : base(default(T), false)
        {
        }
    }
}

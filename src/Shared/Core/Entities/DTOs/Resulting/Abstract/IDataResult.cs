namespace Core.Entities.DTOs
{
    public interface IDataResult<T> : IResult
    {
        T Data { get; }

        int Count { get; }
    }
}

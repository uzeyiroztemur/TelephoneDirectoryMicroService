namespace Core.Entities.DTOs
{
    public interface IResult
    {
        bool Success { get; }

        string Message { get; }
    }
}

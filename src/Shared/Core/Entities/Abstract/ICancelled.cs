namespace Core.Entities.Abstract
{
    public interface ICancelled
    {
        bool IsCancelled { get; set; }
        DateTime? CancelledOn { get; set; }
        int? CancelledBy { get; set; }
    }
}
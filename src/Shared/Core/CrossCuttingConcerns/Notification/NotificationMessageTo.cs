namespace Core.CrossCuttingConcerns.Notification
{
    public class NotificationMessageTo : NotificationMessage
    {
        public List<string> To { get; set; }
        public List<int> Targets { get; set; }
    }
}
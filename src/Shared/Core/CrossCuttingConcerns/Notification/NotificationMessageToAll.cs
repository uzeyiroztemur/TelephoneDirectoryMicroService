namespace Core.CrossCuttingConcerns.Notification
{
    public class NotificationMessageToAll : NotificationMessage
    {
        public NotificationMessageToAll()
        {
            Receivers = new List<string>();
        }

        public IEnumerable<string> Receivers { get; set; }
        public List<int> Targets { get; set; }
    }
}

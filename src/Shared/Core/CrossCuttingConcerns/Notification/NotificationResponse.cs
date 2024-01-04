namespace Core.CrossCuttingConcerns.Notification
{
    public class NotificationResponse
    {
        public long multicast_Id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
    }
}

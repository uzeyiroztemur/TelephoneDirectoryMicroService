namespace Core.CrossCuttingConcerns.Notification
{
    public class NotificationMessage
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string ImgUrl { get; set; }
        public NotificationMessageData Data { get; set; }
    }
}

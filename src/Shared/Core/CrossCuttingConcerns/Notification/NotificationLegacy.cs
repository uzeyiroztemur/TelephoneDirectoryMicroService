namespace Core.CrossCuttingConcerns.Notification
{
    public class NotificationLegacy
    {
        public string title { get; set; }
        public string body { get; set; }
        public string sound => "default";
        public string image => null;
        public int badge => 0;
        public string click_action => "FLUTTER_NOTIFICATION_CLICK";
    }
}

namespace Core.CrossCuttingConcerns.Notification
{
    public class NotificationContentLegacy
    {
        public string to { get; set; }
        public NotificationLegacy notification { get; set; }
        public NotificationContent data { get; set; }
        public string android_channel_id { get; set; }
        public IEnumerable<string> registration_ids { get; set; }

        public bool ShouldSerializeto() => !string.IsNullOrEmpty(to);
        public bool ShouldSerializeregistration_ids() => registration_ids != null && registration_ids.Any();
    }
}

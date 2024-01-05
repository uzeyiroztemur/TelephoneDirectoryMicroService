using Core.Entities.Abstract;

namespace Entities.DTOs.Params
{
    public class DeviceForLoginDTO : IDTO
    {
        public Guid UserId { get; set; }
        public string OS { get; set; }
        public string OSVersion { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public string NotificationToken { get; set; }
        public string DeviceId { get; set; }
    }
}
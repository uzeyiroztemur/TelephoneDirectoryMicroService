using Core.Entities.Abstract;

namespace Entities.DTOs.Params
{
    public class UserForLoginDTO : IDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserDeviceForLoginDTO Device { get; set; }
    }

    public class UserDeviceForLoginDTO : IDTO
    {
        public string OS { get; set; }
        public string OSVersion { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public string NotificationToken { get; set; }
        public string DeviceId { get; set; }
    }
}
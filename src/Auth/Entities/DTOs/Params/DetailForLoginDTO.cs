using Core.Entities.Abstract;

namespace Entities.DTOs.Params
{
    public class DetailForLoginDTO : IDTO
    {
        public Guid UserId { get; set; }
        public Guid UserDeviceId { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
}

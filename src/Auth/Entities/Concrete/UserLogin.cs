using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class UserLogin : IEntity, ICreatable
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DeviceId { get; set; }
        public string IpAddress { get; set; }
        public string UserAgentText { get; set; }
        public string UserAgent { get; set; }
        public string UserAgentVersion { get; set; }

        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual UserDevice Device { get; set; }
        public virtual User User { get; set; }
    }
}

using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class UserDevice : IEntity, IActivable, ICreatable, IModifable, IDeletable
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string IpAddress { get; set; }
        public string OS { get; set; }
        public string OSVersion { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public string BrowserName { get; set; }
        public string NotificationToken { get; set; }
        public string DeviceId { get; set; }

        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid? DeletedBy { get; set; }

        public virtual User User { get; set; }
    }
}

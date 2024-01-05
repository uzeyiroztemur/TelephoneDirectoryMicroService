using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class UserPasswordReset : IEntity, ICreatable
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public DateTime ValidUntil { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public bool? IsChanged { get; set; }

        public virtual User User { get; set; }
    }
}

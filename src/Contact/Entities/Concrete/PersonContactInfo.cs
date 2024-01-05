using Core.Entities.Abstract;
using Entities.Constants;

namespace Entities.Concrete
{
    public class PersonContactInfo : IEntity, IActivable, ICreatable, IModifable, IDeletable
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public ContactInfoType InfoType { get; private set; }
        public string InfoValue { get; private set; }

        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid? DeletedBy { get; set; }

        public virtual Person Person { get; set; }
    }
}

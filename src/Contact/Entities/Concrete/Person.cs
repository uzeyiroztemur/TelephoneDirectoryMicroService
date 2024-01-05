using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class Person : IEntity, IDeletable, IActivable, ICreatable, IModifable
    {
        public Person()
        {
            PersonContacts = new HashSet<PersonContactInfo>();
        }

        public Guid Id { get; set; }
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Company { get; private set; }

        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid? DeletedBy { get; set; }

        public virtual ICollection<PersonContactInfo> PersonContacts { get; set; }
    }
}
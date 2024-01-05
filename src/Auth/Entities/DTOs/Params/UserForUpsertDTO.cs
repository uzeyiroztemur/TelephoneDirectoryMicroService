using Core.Entities.Abstract;

namespace Entities.DTOs.Params
{
    public class UserForUpsertDTO : IDTO
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
    }
}

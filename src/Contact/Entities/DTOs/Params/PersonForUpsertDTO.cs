using Core.Entities.Abstract;

namespace Entities.DTOs.Params
{
    public class PersonForUpsertDTO : IDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public bool IsActive { get; set; }
    }
}
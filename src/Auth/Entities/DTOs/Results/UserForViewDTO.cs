using Core.Entities.Abstract;
using Core.Utilities.Security;

namespace Entities.DTOs.Results
{
    public class UserForViewDTO : IDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AccessToken Token { get; set; }
    }
}

using Entities.Concrete;
using Entities.DTOs.Params;
using Entities.DTOs.Results;
using Core.Entities.DTOs;

namespace Business.Abstract
{
    public interface IUserService
    {
        IResult UpdateAccountLockDown(Guid id, LockdownForAccountDTO lockdownForAccountDTO);
        User GetByUserName(string userName);
        IResult UpdatePassword(Guid userId, byte[] passwordHash, byte[] passwordSalt);
        UserForViewDTO GetByUserNameForView(string userName);
        IResult ChangePassword(PasswordForChangeDTO passwordForChangeDTO);
    }
}

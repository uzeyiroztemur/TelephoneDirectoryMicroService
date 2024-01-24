using Entities.Concrete;
using Entities.DTOs.Params;
using Entities.DTOs.Results;
using Core.Entities.DTOs;

namespace Business.Abstract
{
    public interface IUserService
    {
        Task<IResult> UpdateAccountLockDownAsync(Guid id, LockdownForAccountDTO lockdownForAccountDTO);
        Task<User> GetByUserNameAsync(string userName);
        Task<IResult> UpdatePasswordAsync(Guid userId, byte[] passwordHash, byte[] passwordSalt);
        Task<UserForViewDTO> GetByUserNameForViewAsync(string userName);
        Task<IResult> ChangePasswordAsync(PasswordForChangeDTO passwordForChangeDTO);
    }
}

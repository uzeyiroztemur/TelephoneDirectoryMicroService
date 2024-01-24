using Entities.Concrete;
using Core.Entities.DTOs;

namespace Business.Abstract
{
    public interface IUserPasswordResetService
    {
        Task<IDataResult<UserPasswordReset>> GetAsync(Guid userId);
        Task<IResult> UpdateAsync(byte[] passwordHash);
    }
}

using Entities.Concrete;
using Core.Entities.DTOs;

namespace Business.Abstract
{
    public interface IUserPasswordResetService
    {
        IDataResult<UserPasswordReset> Get(Guid userId);
        IResult Update(byte[] passwordHash, bool? isChanged);
    }
}

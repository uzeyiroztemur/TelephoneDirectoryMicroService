using Business.Abstract;
using Business.Constants;
using Core.Entities.DTOs;
using Core.Extensions;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class UserPasswordResetManager : IUserPasswordResetService
    {
        private readonly IUserPasswordResetDal _passwordResetDal;

        public UserPasswordResetManager(IUserPasswordResetDal passwordResetDal)
        {
            _passwordResetDal = passwordResetDal;
        }

        public async Task<IDataResult<UserPasswordReset>> GetAsync(Guid userId)
        {
            var result = await _passwordResetDal.GetLastRequestAsync(userId);
            if (result.NotNull())
                return new SuccessDataResult<UserPasswordReset>(result);

            return new ErrorDataResult<UserPasswordReset>(Messages.RecordNotFound);
        }

        public async Task<IResult> UpdateAsync(byte[] passwordHash)
        {
            var update = await _passwordResetDal.GetAsync(x => x.PasswordHash == passwordHash);
            if (update != null)
            {
                update.IsChanged = true;
                await _passwordResetDal.UpdateAsync(update);
                return new SuccessResult();
            }

            return new ErrorResult();
        }
    }
}

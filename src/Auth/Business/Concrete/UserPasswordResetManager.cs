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

        public IDataResult<UserPasswordReset> Get(Guid userId)
        {
            var result = _passwordResetDal.GetLastRequest(userId);
            if (result.NotNull())
            {
                return new SuccessDataResult<UserPasswordReset>(result);
            }

            return new ErrorDataResult<UserPasswordReset>(Messages.RecordNotFound);
        }

        public IResult Update(byte[] passwordHash,bool? isChanged)
        {
            var update = _passwordResetDal.Get(x => x.PasswordHash == passwordHash);
            if (update != null)
            {
                update.IsChanged = true;
                _passwordResetDal.Update(update);
                return new SuccessResult();
            }

            return new ErrorResult();
        }
    }
}

using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation.Concrete;
using Core.Aspects.Autofac.Validation;
using Core.Entities.DTOs;
using Core.Extensions;
using Core.Utilities.Business;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Params;
using Entities.DTOs.Results;

namespace Business.Concrete
{
    public class UserManager : BaseManager, IUserService
    {
        private readonly IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        #region Business Rules
        private static IResult ValidatePassword(User user, string oldPassword, string newPassword, string newPasswordAgain)
        {
            if (newPassword != newPasswordAgain)
                return new ErrorResult(Messages.PasswordMatch);

            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            if (!HashingHelper.VerifyPasswordHash(oldPassword, user.PasswordHash, user.PasswordSalt))
                return new ErrorResult(Messages.PasswordDoesNotMatch);

            var minimumPasswordLength = 8; //Parametreden alınabilir.
            if (newPassword.Length < minimumPasswordLength)
                return new ErrorResult(string.Format(Messages.PasswordDoesNotLength, minimumPasswordLength.ToString()));

            return new SuccessResult();
        }
        #endregion

        public async Task<User> GetAsync(Guid id)
        {
            return await _userDal.GetAsync(f => f.Id == id && f.IsActive);
        }

        public async Task<IResult> UpdateAccountLockDownAsync(Guid id, LockdownForAccountDTO lockdownForAccountDTO)
        {
            var user = await _userDal.GetAsync(f => f.Id == id && f.IsActive);
            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            user.AccessFailedCount = lockdownForAccountDTO.AccessFailedCount;
            user.LockoutEnabled = lockdownForAccountDTO.LockoutEnabled;
            user.LockoutEndDateUtc = lockdownForAccountDTO.LockoutEndDateUtc;

            await _userDal.UpdateAsync(user);

            return new SuccessResult();
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await _userDal.GetAsync(f => (f.UserName == userName) && f.IsActive);
        }

        public async Task<IResult> UpdatePasswordAsync(Guid userId, byte[] passwordHash, byte[] passwordSalt)
        {
            var userToUpdate = await _userDal.GetAsync(g => g.Id == userId);
            if (userToUpdate.IsNull())
                return new ErrorResult(Messages.UserNotFound);

            userToUpdate.PasswordHash = passwordHash;
            userToUpdate.PasswordSalt = passwordSalt;
            userToUpdate.LastPasswordChangeDate = DateTime.Now;

            userToUpdate.ModifiedBy = UserId;
            userToUpdate.ModifiedOn = DateTime.Now;

            await _userDal.UpdateAsync(userToUpdate);

            return new SuccessResult();
        }

        public async Task<UserForViewDTO> GetByUserNameForViewAsync(string userName)
        {
            return await _userDal.GetByUserNameAsync(userName);
        }

        [ValidationAspect(typeof(PasswordForChangeValidator), Priority = 1)]
        public async Task<IResult> ChangePasswordAsync(PasswordForChangeDTO passwordForChangeDTO)
        {
            var userToUpdate = await GetAsync(UserId);
            if (userToUpdate == null)
                return new ErrorResult(Messages.UserNotFound);

            var result = BusinessRules.Run(ValidatePassword(userToUpdate, passwordForChangeDTO.OldPassword, passwordForChangeDTO.NewPassword, passwordForChangeDTO.NewPasswordAgain));
            if (result != null)
                return new ErrorResult(result.Message);

            HashingHelper.CreatePasswordHash(passwordForChangeDTO.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);

            userToUpdate.PasswordHash = passwordHash;
            userToUpdate.PasswordSalt = passwordSalt;
            userToUpdate.LastPasswordChangeDate = DateTime.Now;

            userToUpdate.ModifiedBy = UserId;
            userToUpdate.ModifiedOn = DateTime.Now;

            await _userDal.UpdateAsync(userToUpdate);

            return new SuccessResult(Messages.UserPasswordChanged);
        }
    }
}
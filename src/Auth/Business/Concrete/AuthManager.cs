using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation.Concrete;
using Core.Aspects.Autofac.Validation;
using Core.Entities.DTOs;
using Core.Extensions;
using Core.Utilities.Security;
using Core.Utilities.Security.Hashing;
using Entities.Concrete;
using Entities.DTOs.Params;
using Entities.DTOs.Results;
using UAParser;

namespace Business.Concrete
{
    public class AuthManager : BaseManager, IAuthService
    {
        private readonly IUserService _userService;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserDeviceService _userDeviceService;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenHelper _tokenHelper;
        private readonly IUserPasswordResetService _passwordResetService;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper, IUserLoginService userLoginService, IUserDeviceService userDeviceService, IUserPasswordResetService passwordResetService, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _userLoginService = userLoginService;
            _userDeviceService = userDeviceService;
            _passwordResetService = passwordResetService;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Helper
        private async Task<IResult> HandleLockdown(User user)
        {
            var lockdownTime = 15;
            var accessFailedLimit = 5; //Parametreden alınabilir.

            if (++user.AccessFailedCount >= accessFailedLimit)
            {
                user.LockoutEnabled = true;
                user.LockoutEndDateUtc = DateTime.UtcNow.AddMinutes(lockdownTime);
            }

            var lockdownResult = await _userService.UpdateAccountLockDownAsync(user.Id, new LockdownForAccountDTO
            {
                AccessFailedCount = user.AccessFailedCount,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEndDateUtc = user.LockoutEndDateUtc
            });

            if (lockdownResult.Success)
            {
                if (user.LockoutEnabled)
                    return new ErrorResult(Messages.UserAccountLockeddown.Replace("{Minutes}", lockdownTime.ToString()));

                return new SuccessResult();
            }
            else
            {
                return new ErrorResult(lockdownResult.Message);
            }
        }
        private IResult LockdownError(DateTime lockoutEntDateUtc)
        {
            var diff = lockoutEntDateUtc - DateTime.UtcNow;
            var minutesLeft = Math.Round(diff.TotalMinutes);
            return new ErrorResult(Messages.UserAccountLockeddown.Replace("{Minutes}", minutesLeft.ToString()));
        }
        private async Task<IResult> CheckLockdown(User user)
        {
            if (user.LockoutEnabled)
            {
                if (user.LockoutEndDateUtc <= DateTime.UtcNow)
                {
                    user.LockoutEnabled = false;
                    user.LockoutEndDateUtc = null;
                    user.AccessFailedCount = 0;

                    var lockdownResult = await _userService.UpdateAccountLockDownAsync(user.Id, new LockdownForAccountDTO
                    {
                        AccessFailedCount = user.AccessFailedCount,
                        LockoutEnabled = user.LockoutEnabled,
                        LockoutEndDateUtc = user.LockoutEndDateUtc
                    });

                    if (!lockdownResult.Success)
                        return new ErrorResult(lockdownResult.Message);
                }
                else
                {
                    return LockdownError((DateTime)user.LockoutEndDateUtc);
                }
            }

            return new SuccessResult();
        }

        private ClientInfo ParseUserAgent(string userAgent)
        {
            var uaParser = Parser.GetDefault();
            return uaParser.Parse(userAgent);
        }
        private string GetVersion(string[] versions)
        {
            return string.Join(".", versions);
        }
        #endregion

        [ValidationAspect(typeof(UserForLoginValidator), Priority = 1)]
        public async Task<IDataResult<UserForViewDTO>> LoginAsync(UserForLoginDTO userForLoginDTO)
        {
            var userToCheck = await _userService.GetByUserNameAsync(userForLoginDTO.UserName);
            if (userToCheck == null)
                return new ErrorDataResult<UserForViewDTO>(Messages.UserNotFound);

            // Şifresi olmayan kullanıcının sisteme girişi engellenir.
            if (userToCheck.PasswordHash == null || userToCheck.PasswordSalt == null)
                return new ErrorDataResult<UserForViewDTO>(Messages.PasswordUndefined);

            // hesap kilit kontrolü
            var lockdownResult = await CheckLockdown(userToCheck);
            if (!lockdownResult.Success)
                return new ErrorDataResult<UserForViewDTO>(lockdownResult.Message);

            // Yanlış şifre kontrolü
            var passwordMatch = false;
            if (!HashingHelper.VerifyPasswordHash(userForLoginDTO.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                var resetResult = await _passwordResetService.GetAsync(userToCheck.Id);
                if (resetResult.Success)
                {
                    if (HashingHelper.VerifyPasswordHash(userForLoginDTO.Password, resetResult.Data.PasswordHash, resetResult.Data.PasswordSalt))
                    {
                        if (resetResult.Data.IsChanged != true)
                        {
                            await _passwordResetService.UpdateAsync(resetResult.Data.PasswordHash);

                            var updateResult = await _userService.UpdatePasswordAsync(userToCheck.Id, resetResult.Data.PasswordHash, resetResult.Data.PasswordSalt);
                            if (updateResult.Success)
                                passwordMatch = true;
                        }

                    }
                }
            }
            else
            {
                passwordMatch = true;
            }

            // Yanlış şifrede hesap kilitlenme kontrolü
            if (!passwordMatch)
            {
                var lockdownHandleResult = await HandleLockdown(userToCheck);
                if (!lockdownHandleResult.Success)
                    return new ErrorDataResult<UserForViewDTO>(lockdownHandleResult.Message);

                return new ErrorDataResult<UserForViewDTO>(Messages.PasswordInvalid);
            }
            else
            {
                await _userService.UpdateAccountLockDownAsync(userToCheck.Id, new LockdownForAccountDTO { AccessFailedCount = 0, LockoutEnabled = false, LockoutEndDateUtc = null });
            }

            // Son şifre değiştirme sıklığı kontrolü
            var user = await _userService.GetByUserNameAsync(userForLoginDTO.UserName);
            var passwordChangeFrequency = 120; //Parametreden getirilebilir.
            var dateDiff = DateTime.Now - user.LastPasswordChangeDate;
            if (dateDiff.TotalDays >= passwordChangeFrequency)
                return new ErrorDataResult<UserForViewDTO>(Messages.UserMustChangePassword);

            var userToReturn = await _userService.GetByUserNameForViewAsync(userForLoginDTO.UserName);
            if (userToReturn == null)
                return new ErrorDataResult<UserForViewDTO>(Messages.UserNotFound);

            //Cihaz Bilgileri
            var ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            var userAgent = _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString();

            var clientInfo = ParseUserAgent(userAgent);
            if (clientInfo.IsNull())
                return new ErrorDataResult<UserForViewDTO>(Messages.UserAgentParseError);

            //Cihaz Kayıt
            DeviceForLoginDTO deviceForLogin;

            if (userForLoginDTO.Device != null)
            {
                deviceForLogin = new DeviceForLoginDTO
                {
                    UserId = userToReturn.Id,
                    OS = userForLoginDTO.Device.OS,
                    OSVersion = userForLoginDTO.Device.OSVersion,
                    Model = userForLoginDTO.Device.Model,
                    Name = userForLoginDTO.Device.Name,
                    NotificationToken = userForLoginDTO.Device.NotificationToken,
                    DeviceId = userForLoginDTO.Device.DeviceId,
                };
            }
            else
            {
                deviceForLogin = new DeviceForLoginDTO
                {
                    UserId = userToReturn.Id,
                    OS = clientInfo.OS.Family.NotEmpty() ? clientInfo.OS.Family : null,
                    OSVersion = GetVersion(new[] { clientInfo.OS?.Major ?? "0", clientInfo.OS?.Minor ?? "0" }) ?? null,
                    Model = null,
                    Name = null,
                    NotificationToken = null,
                    DeviceId = null,
                };
            }

            var browserName = clientInfo.UA.Family.NotEmpty() ? clientInfo.UA.Family : null;

            var deviceResult = await _userDeviceService.UpsertAsync(deviceForLogin, ipAddress, browserName);
            if (!deviceResult.Success)
                return new ErrorDataResult<UserForViewDTO>(deviceResult.Message);

            //Cihaz kayıt kontrolü
            if (deviceResult.Data == null)
                return new ErrorDataResult<UserForViewDTO>(Messages.UserDeviceNotRegistered);

            if (deviceResult.Data.IsActive == false)
                return new ErrorDataResult<UserForViewDTO>(Messages.UserDeviceNotRegistered);

            var detailForLogin = new DetailForLoginDTO
            {
                IpAddress = ipAddress,
                UserAgent = userAgent,
                UserId = userToReturn.Id,
                UserDeviceId = deviceResult.Data.Id,
            };

            var logResult = await _userLoginService.LogAsync(detailForLogin);
            if (!logResult.Success)
                return new ErrorDataResult<UserForViewDTO>(logResult.Message);

            return new SuccessDataResult<UserForViewDTO>(userToReturn, Messages.UserSuccessfullLogin);
        }

        public async Task<IDataResult<AccessToken>> CreateAccessTokenAsync(UserForViewDTO userForViewDTO)
        {
            var accessToken = _tokenHelper.CreateToken(new UserClaims
            {
                Id = userForViewDTO.Id,
                Name = userForViewDTO.Name,
            });

            return await Task.FromResult(new SuccessDataResult<AccessToken>(accessToken, Messages.UserAccessTokenCreated));
        }

        [ValidationAspect(typeof(PasswordForChangeValidator), Priority = 1)]
        public async Task<IResult> ChangePasswordAsync(PasswordForChangeDTO passwordForChangeDTO)
        {
            return await _userService.ChangePasswordAsync(passwordForChangeDTO);
        }
    }
}
